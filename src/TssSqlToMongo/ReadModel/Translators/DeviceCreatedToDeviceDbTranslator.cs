namespace TssSqlToMongo.ReadModel.Translators
{
    using System;
    using System.Collections.Generic;

    using TssSqlToMongo.Core;
    using TssSqlToMongo.Data.Entities;
    using TssSqlToMongo.ReadModel.Events;

    public class DeviceCreatedToDeviceDbTranslator : ITranslator<DeviceCreated, DeviceDb>
    {
        public DeviceDb Translate(DeviceCreated @from)
        {
            return new DeviceDb()
            {
                Id = @from.AggregateId,
                Version = @from.Version,
                CreatedTimestamp = @from.Timestamp.ToUniversalTime(),
                CreatedByUserId = @from.UserId,
                LastModifiedTimestamp = @from.Timestamp.ToUniversalTime(),
                LastModifiedByUserId = @from.UserId,
                Name = @from.Name,
                NameInsensitive = @from.Name?.ToLower(),
                IpAddress = @from.IpAddress,
                DeviceType = @from.DeviceType,
                DeviceTypeInsensitive = @from.DeviceType?.ToLower(),
                ModBusId = @from.ModBusId,
                IsCheckInOrOut = @from.IsCheckInOrOut,
                ExternalId = @from.ExternalId,
                MacAddress = @from.MacAddress?.ToLower(),
                AccessType = @from.AccessType,
                LocationName = @from.LocationName,
                HasMaglock = @from.HasMaglock,
                Readers = new List<ReaderDb>()
            };
        }

        public DeviceDb Translate(DeviceCreated t, DeviceDb tr)
        {
            throw new NotImplementedException();
        }
    }
}