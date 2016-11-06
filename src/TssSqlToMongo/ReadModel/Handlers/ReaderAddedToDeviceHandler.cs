namespace ConsoleApplication2.ReadModel.Handlers
{
    using System;
    using System.Diagnostics;

    using ConsoleApplication2.Core;
    using ConsoleApplication2.ReadModel.Events;
    using ConsoleApplication2.ReadModel.Services;

    public class ReaderAddedToDeviceHandler : IEventHandler<ReaderAddedToDevice>
    {
        private readonly IDevicesWriteService sitesWriteService;

        public ReaderAddedToDeviceHandler(IDevicesWriteService sitesWriteService)
        {
            this.sitesWriteService = sitesWriteService;
        }

        public void Handle(ReaderAddedToDevice message)
        {
            try
            {
                Debug.WriteLine($"{nameof(message)}: {message.AggregateId} / {message.Version}");

                this.sitesWriteService.Update(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public interface IEventHandler<T> : IHandler<T>
       where T : IEvent
    {
    }

    public interface IHandler<in T>
    {
        void Handle(T message);
    }
}