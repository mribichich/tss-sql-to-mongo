namespace ConsoleApplication2.ReadModel.Services
{
    using ConsoleApplication2.ReadModel.Events;

    public interface IDevicesWriteService
    {
        void Add(DeviceCreated message);

        void Update(ReaderAddedToDevice message);

        void Update(DeviceUpdatedFromDeviceInfo message);
    }
}