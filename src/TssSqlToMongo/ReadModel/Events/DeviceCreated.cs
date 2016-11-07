namespace TssSqlToMongo.ReadModel.Events
{
    using System;

    public class DeviceCreated : BaseDevicesEvent
    {
        public DeviceCreated(
            Guid aggregateId,
            string userId,
            string name,
            string ipAddress,
            string deviceType,
            int modBusId,
            ////short? mode,
            bool isCheckInOrOut,
            int? externalId,
            string macAddress,
            int? accessType,
            string locationName,
            bool? hasMaglock)
            : base(aggregateId, userId)
        {
            this.Name = name;
            this.IpAddress = ipAddress;
            this.DeviceType = deviceType;
            this.ModBusId = modBusId;
            ////this.Mode = mode;
            this.IsCheckInOrOut = isCheckInOrOut;
            this.ExternalId = externalId;
            this.MacAddress = macAddress;
            this.AccessType = accessType;
            this.LocationName = locationName;
            this.HasMaglock = hasMaglock;
        }

        public string Name { get; protected set; }

        public string IpAddress { get; protected set; }

        public string DeviceType { get; protected set; }

        public int ModBusId { get; protected set; }

        ////public short? Mode { get; protected set; }

        public bool IsCheckInOrOut { get; protected set; }

        public int? ExternalId { get; protected set; }

        public string MacAddress { get; protected set; }

        public int? AccessType { get; protected set; }

        public string LocationName { get; protected set; }

        public bool? HasMaglock { get; protected set; }
    }
}