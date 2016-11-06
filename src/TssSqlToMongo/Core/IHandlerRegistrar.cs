namespace ConsoleApplication2.Core
{
    using System;

    public interface IHandlerRegistrar
    {
        void RegisterHandler<T>(Action<T> handler)
            where T : IMessage;
    }
}