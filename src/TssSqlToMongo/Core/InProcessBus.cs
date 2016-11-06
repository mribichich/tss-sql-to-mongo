namespace ConsoleApplication2.Core
{
    using System;
    using System.Collections.Generic;

    public class InProcessBus : ICommandSender,
                                IEventPublisher,
                                IHandlerRegistrar
    {
        private readonly Dictionary<Type, List<Action<IMessage>>> routes =
            new Dictionary<Type, List<Action<IMessage>>>();

        public void RegisterHandler<T>(Action<T> handler)
            where T : IMessage
        {
            List<Action<IMessage>> handlers;

            if (!this.routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<IMessage>>();
                this.routes.Add(typeof(T), handlers);
            }

            handlers.Add(x => handler((T)x));
        }

        public void Send<T>(T command)
            where T : ICommand
        {
            this.HandleCommand(command);
        }

        public void Publish<T>(T @event)
            where T : IEvent
        {
            this.PublishInternal(@event);

            ////AsyncHelpers.RunSync(() => this.PublishInternalAsync(@event));
        }

        private void HandleCommand<T>(T command)
            where T : ICommand
        {
            List<Action<IMessage>> handlers;

            if (this.routes.TryGetValue(command.GetType(), out handlers))
            {
                if (handlers.Count != 1)
                {
                    throw new InvalidOperationException("Cannot send to more than one handler");
                }

                handlers[0](command);
            }
            else
            {
                throw new InvalidOperationException("No handler registered");
            }
        }

        private void PublishInternal<T>(T @event)
            where T : IEvent
        {
            List<Action<IMessage>> handlers;

            if (!this.routes.TryGetValue(@event.GetType(), out handlers))
            {
                return;
            }

            foreach (var handler in handlers)
            {
                handler(@event);
            }
        }
    }
}