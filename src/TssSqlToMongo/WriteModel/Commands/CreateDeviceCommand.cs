namespace ConsoleApplication2.WriteModel.Commands
{
    using System;
    using System.Collections.Generic;

    using ConsoleApplication2.Core;
    using ConsoleApplication2.ValueObjects;

    public class CreateDeviceCommand : BaseCommand
    {
        public CreateDeviceCommand(
            Guid id,
            string userId,
            string name,
            string ipAddress,
            string deviceType,
            int modBusId,
            bool isCheckInOrOut,
            int? externalId,
            string macAddress,
            IEnumerable<Reader> readers,
            int? accessType,
            string locationName,
            bool? hasMaglock)
            : base(id, 0, userId)
        {
            this.Name = name;
            this.IpAddress = ipAddress;
            this.DeviceType = deviceType;
            this.ModBusId = modBusId;
            this.IsCheckInOrOut = isCheckInOrOut;
            this.ExternalId = externalId;
            this.MacAddress = macAddress;
           this.Readers = readers;
            this.AccessType = accessType;
            this.LocationName = locationName;
            this.HasMaglock = hasMaglock;
        }

        public string Name { get; }

        public string IpAddress { get; }

        public string DeviceType { get; }

        public int ModBusId { get; }

        public bool IsCheckInOrOut { get; }

        public int? ExternalId { get; }

        public string MacAddress { get; }

       public IEnumerable<Reader> Readers { get; }

        public int? AccessType { get; }

        public string LocationName { get; }

        public bool? HasMaglock { get; }
    }
}