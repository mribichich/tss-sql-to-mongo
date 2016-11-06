namespace ConsoleApplication2.ReadModel.Translators
{
    using ConsoleApplication2.Core;
    using ConsoleApplication2.Data.Entities;
    using ConsoleApplication2.ReadModel.Events;

    using MongoDB.Driver;

    public class ReaderAddedToDeviceToUpdateDefinitionTranslator : ITranslator<ReaderAddedToDevice, UpdateDefinition<DeviceDb>>
    {
        private readonly ITranslator<ReaderAddedToDevice, ReaderDb> readerAddedToDeviceTranslator;

        public ReaderAddedToDeviceToUpdateDefinitionTranslator(ITranslator<ReaderAddedToDevice, ReaderDb> readerAddedToDeviceTranslator)
        {
            this.readerAddedToDeviceTranslator = readerAddedToDeviceTranslator;
        }

        public UpdateDefinition<DeviceDb> Translate(ReaderAddedToDevice @from)
        {
            return Builders<DeviceDb>.Update
                .Set(g => g.Version, @from.Version)
                .Set(g => g.LastModifiedTimestamp, @from.Timestamp)
                .Set(g => g.LastModifiedByUserId, @from.UserId)
                .AddToSet(g => g.Readers, this.readerAddedToDeviceTranslator.Translate(@from));
        }

        public UpdateDefinition<DeviceDb> Translate(ReaderAddedToDevice @from, UpdateDefinition<DeviceDb> tr)
        {
            throw new System.NotImplementedException();
        }
    }
}