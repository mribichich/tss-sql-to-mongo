namespace TssSqlToMongo.ReadModel.Handlers
{
    using System;
    using System.Diagnostics;

    using TssSqlToMongo.ReadModel.Events;
    using TssSqlToMongo.ReadModel.Services;

    public class DeviceCreatedHandler : IEventHandler<DeviceCreated>
    {
        private readonly IDevicesWriteService devicesWriteService;

        public DeviceCreatedHandler(
            IDevicesWriteService devicesWriteService)
        {
            this.devicesWriteService = devicesWriteService;
        }

        public void Handle(DeviceCreated message)
        {
            try
            {
                Debug.WriteLine($"{nameof(message)}: {message.AggregateId} / {message.Version}");

                this.devicesWriteService.Add(message);
            }
            catch (Exception e)
            {
                  Console.WriteLine(e);
            }
        }
    }
}