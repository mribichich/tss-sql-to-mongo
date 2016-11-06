namespace ConsoleApplication2.ReadModel.Translators
{
    using ConsoleApplication2.Core;
    using ConsoleApplication2.Data.Entities;
    using ConsoleApplication2.ReadModel.Events;

    using MongoDB.Driver;

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