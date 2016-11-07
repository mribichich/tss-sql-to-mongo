namespace TssSqlToMongo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Autofac;

    using TssSqlToMongo.ReadModel.Handlers;

    public class BusRegistrar
    {
        private readonly IContainer serviceLocator;
        private static readonly Type CommandHandlerType = typeof(ICommandHandler<>);
        private static readonly Type EventHandlerType = typeof(IEventHandler<>);

        public BusRegistrar(IContainer serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void Register(params Type[] typesFromAssemblyContainingMessages)
        {
            var bus = this.serviceLocator.Resolve<IHandlerRegistrar>();

            foreach (var executorsAssembly in typesFromAssemblyContainingMessages.Select(s => s.Assembly)
                .Distinct())
            {
                var executorTypes = executorsAssembly.GetTypes()
                    .Select(
                        t => new
                        {
                            Type = t,
                            Interfaces = ResolveMessageHandlerInterface(t)
                        })
                    .Where(e => e.Interfaces != null && e.Interfaces.Any());

                foreach (var executorType in executorTypes)
                {
                    foreach (var @interface in executorType.Interfaces)
                    {
                        this.InvokeHandler(@interface, bus, executorType.Type);
                    }
                }
            }
        }

        private static IEnumerable<Type> ResolveMessageHandlerInterface(Type type)
        {
            return type.GetInterfaces()
                .Where(
                    i =>
                    i.IsGenericType
                    && ((i.GetGenericTypeDefinition() == CommandHandlerType)
                        || i.GetGenericTypeDefinition() == EventHandlerType));
        }

        private void InvokeHandler(Type @interface, IHandlerRegistrar bus, Type executorType)
        {
            var commandType = @interface.GetGenericArguments()[0];

            var registerExecutorMethod = bus.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(mi => mi.Name == "RegisterHandler")
                .Where(mi => mi.IsGenericMethod)
                .Where(mi => mi.GetGenericArguments().Count() == 1)
                .Single(mi => mi.GetParameters().Count() == 1)
                .MakeGenericMethod(commandType);

            Action<dynamic> action;

            if (@interface.GetGenericTypeDefinition() == CommandHandlerType)
            {
                action = command =>
                    {
                        Type transType = typeof(TransactionalHandler<>).MakeGenericType(command.GetType());

                        ////dynamic b = serviceLocator.Resolve(typeof(TransactionalHandler<CreateSiteCommand>));

                        dynamic transactionalHandler = this.serviceLocator.Resolve(transType);

                        transactionalHandler.Handle(command);
                    };
            }
            else if (@interface.GetGenericTypeDefinition() == EventHandlerType)
            {
                action = rootEvent =>
                    {
                        dynamic handler = this.serviceLocator.Resolve(executorType);
                        handler.Handle(rootEvent);
                    };
            }
            else
            {
                throw new IndexOutOfRangeException();
            }

            registerExecutorMethod.Invoke(bus, new object[] { action });
        }
    }
}