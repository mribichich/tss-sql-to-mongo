namespace TssSqlToMongo.ReadModel.Translators
{
    using MongoDB.Driver;

    using TssSqlToMongo.Core;
    using TssSqlToMongo.Data.Entities;
    using TssSqlToMongo.ReadModel.Events;

    public class DeviceUpdatedFromDeviceInfoToUpdateDefinitionTranslator : ITranslator<DeviceUpdatedFromDeviceInfo, UpdateDefinition<DeviceDb>>
    {
        public UpdateDefinition<DeviceDb> Translate(DeviceUpdatedFromDeviceInfo @from)
        {
            return Builders<DeviceDb>.Update
                .Set(g => g.Version, @from.Version)
                .Set(g => g.LastModifiedTimestamp, @from.Timestamp)
                .Set(g => g.LastModifiedByUserId, @from.UserId)
                .Set(g => g.Type, @from.Type)
                .Set(g => g.DeviceVersion, @from.DeviceVersion);
        }

        public UpdateDefinition<DeviceDb> Translate(DeviceUpdatedFromDeviceInfo @from, UpdateDefinition<DeviceDb> tr)
        {
            throw new System.NotImplementedException();
        }
    }
}