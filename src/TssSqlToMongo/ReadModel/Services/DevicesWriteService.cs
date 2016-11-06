namespace ConsoleApplication2.ReadModel.Services
{
    using ConsoleApplication2.Core;
    using ConsoleApplication2.Data.Entities;
    using ConsoleApplication2.Data.UnitOfWorks;
    using ConsoleApplication2.ReadModel.Events;
    using ConsoleApplication2.ReadModel.Translators;

    using MongoDB.Driver;

    public class DevicesWriteService : IDevicesWriteService
    {
        private readonly IDataStore dataStore;
        private readonly ITranslator<DeviceCreated, DeviceDb> deviceCreateTranslator;
        private readonly ITranslator<ReaderAddedToDevice, UpdateDefinition<DeviceDb>> readerAddedToDeviceTranslator;
        private readonly ITranslator<DeviceUpdatedFromDeviceInfo, UpdateDefinition<DeviceDb>> deviceUpdatedFromDeviceInfoTranslator;
        ////private readonly ITranslator<ReaderRemovedFromDevice, UpdateDefinition<DeviceDb>> readerRemovedFromDeviceTranslator;

        public DevicesWriteService(
            IDataStore dataStore,
            ITranslator<DeviceCreated, DeviceDb> deviceCreateTranslator,
            ITranslator<ReaderAddedToDevice, UpdateDefinition<DeviceDb>> readerAddedToDeviceTranslator,
            ITranslator<DeviceUpdatedFromDeviceInfo, UpdateDefinition<DeviceDb>> deviceUpdatedFromDeviceInfoTranslator
            ////ITranslator<ReaderRemovedFromDevice, UpdateDefinition<DeviceDb>> readerRemovedFromDeviceTranslator
            )
        {
            this.dataStore = dataStore;
            this.deviceCreateTranslator = deviceCreateTranslator;
            this.readerAddedToDeviceTranslator = readerAddedToDeviceTranslator;
            this.deviceUpdatedFromDeviceInfoTranslator = deviceUpdatedFromDeviceInfoTranslator;
            ////this.readerRemovedFromDeviceTranslator = readerRemovedFromDeviceTranslator;
        }

        public void Add(DeviceCreated message)
        {
            var deviceDb = this.deviceCreateTranslator.Translate(message);

            this.dataStore.Insert(deviceDb);
        }
        
        public void Update(ReaderAddedToDevice message)
        {
            var updateDefinition = this.readerAddedToDeviceTranslator.Translate(message);

            this.dataStore.FindOneAndUpdate(g => g.Id == message.AggregateId, updateDefinition);
        }

        public void Update(DeviceUpdatedFromDeviceInfo message)
        {
            var updateDefinition = this.deviceUpdatedFromDeviceInfoTranslator.Translate(message);

            this.dataStore.FindOneAndUpdate(g => g.Id == message.AggregateId, updateDefinition);
        }

        ////public void Update(ReaderRemovedFromDevice message)
        ////{
        ////    var updateDefinition = this.readerRemovedFromDeviceTranslator.Translate(message);

        ////    this.dataStore.FindOneAndUpdate(g => g.Id == message.AggregateId, updateDefinition);
        ////}
    }
}