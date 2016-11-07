namespace TssSqlToMongo.ReadModel.Services
{
    using TssSqlToMongo.ReadModel.Events;

    public interface IDevicesWriteService
    {
        void Add(DeviceCreated message);

        void Update(ReaderAddedToDevice message);

        void Update(DeviceUpdatedFromDeviceInfo message);
    }
}