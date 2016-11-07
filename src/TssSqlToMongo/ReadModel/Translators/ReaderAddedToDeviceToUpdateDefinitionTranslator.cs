namespace TssSqlToMongo.ReadModel.Translators
{
    using MongoDB.Driver;

    using TssSqlToMongo.Core;
    using TssSqlToMongo.Data.Entities;
    using TssSqlToMongo.ReadModel.Events;

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