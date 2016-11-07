namespace TssSqlToMongo.Sql
{
    using System;
    using System.Collections.Generic;

    public class DeviceSql
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public string DeviceType { get; set; }

        public int ModBusId { get; set; }

        public bool IsCheckInOrOut { get; set; }

        public int ExternalId { get; set; }

        public string MacAddress { get; set; }

        public string DeviceVersion { get; set; }

        public int? Type { get; set; }

        public List<ReaderSql> Readers { get; set; }

        public int? AccessType { get; set; }

        public string LocationName { get; set; }

        public bool? HasMaglock { get; set; }
    }
}