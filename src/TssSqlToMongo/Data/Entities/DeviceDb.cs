namespace TssSqlToMongo.Data.Entities
{
    using System;
    using System.Collections.Generic;

    public class DeviceDb : IDbEntity
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public DateTime CreatedTimestamp { get; set; }

        public string CreatedByUserId { get; set; }

        public DateTime? LastModifiedTimestamp { get; set; }

        public string LastModifiedByUserId { get; set; }

        public string Name { get; set; }

        public string NameInsensitive { get; set; }

        public string IpAddress { get; set; }

        public string DeviceType { get; set; }

        public string DeviceTypeInsensitive { get; set; }

        public int ModBusId { get; set; }

        ////public short? Mode { get; set; }

        ////public bool? Status { get; set; }

        public bool IsCheckInOrOut { get; set; }

        public int? ExternalId { get; set; }

        public string MacAddress { get; set; }

        public string DeviceVersion { get; set; }

        public int? Type { get; set; }

        public IList<ReaderDb> Readers { get; set; }

        public int? AccessType { get; set; }

        public string LocationName { get; set; }

        public bool? HasMaglock { get; set; }
    }
}