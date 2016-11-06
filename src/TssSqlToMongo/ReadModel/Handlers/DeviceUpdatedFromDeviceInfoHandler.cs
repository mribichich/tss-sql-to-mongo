namespace ConsoleApplication2.ReadModel.Handlers
{
    using System;
    using System.Diagnostics;

    using ConsoleApplication2.ReadModel.Events;
    using ConsoleApplication2.ReadModel.Services;

    public class DeviceUpdatedFromDeviceInfoHandler : IEventHandler<DeviceUpdatedFromDeviceInfo>
    {
        private readonly IDevicesWriteService devicesWriteService;

        public DeviceUpdatedFromDeviceInfoHandler(IDevicesWriteService devicesWriteService)
        {
            this.devicesWriteService = devicesWriteService;
        }

        public void Handle(DeviceUpdatedFromDeviceInfo message)
        {
            try
            {
                Debug.WriteLine($"{nameof(message)}: {message.AggregateId} / {message.Version}");

                this.devicesWriteService.Update(message);
            }
            catch (Exception e)
            {
                  Console.WriteLine(e);
            }
        }
    }
}