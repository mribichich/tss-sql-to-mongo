namespace TssSqlToMongo.AggregateRoots
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TssSqlToMongo.Core;
    using TssSqlToMongo.ReadModel.Events;
    using TssSqlToMongo.ValueObjects;

    public class Device : AggregateRoot,
                          IApplyEvent<DeviceCreated>,
                          IApplyEvent<ReaderAddedToDevice>,
                          IApplyEvent<DeviceUpdatedFromDeviceInfo>
    {
        public Device(
            Guid id,
            string userId,
            string name,
            string ipAddress,
            string deviceType,
            int modBusId,
            bool isCheckInOrOut,
            int? externalId,
            string macAddress,
            int? accessType,
            string locationName,
            bool? hasMaglock)
        {
            this.ApplyChange(
                new DeviceCreated(
                    id,
                    userId,
                    name,
                    ipAddress,
                    deviceType,
                    modBusId,
                    ////mode,
                    isCheckInOrOut,
                    externalId,
                    macAddress,
                    accessType,
                    locationName,
                    hasMaglock));
        }

        protected Device()
        {
        }

        public string Name { get; protected set; }

        public string IpAddress { get; protected set; }

        public string DeviceType { get; protected set; }

        public int ModBusId { get; protected set; }

        public bool IsCheckInOrOut { get; protected set; }

        public int? ExternalId { get; protected set; }

        public string MacAddress { get; protected set; }

        public string DeviceVersion { get; protected set; }

        public int? Type { get; protected set; }

        public List<Reader> Readers { get; protected set; } = new List<Reader>();

        public int? AccessType { get; protected set; }

        public string LocationName { get; protected set; }

        public bool? HasMaglock { get; protected set; }

        public bool IsDeleted { get; protected set; }
        
        public void AddReader(string userId, Guid readerId, int number)
        {
            if (this.Readers.Any(a => a.Number == number))
            {
                throw new InvalidOperationException("already added");
            }

            this.ApplyChange(new ReaderAddedToDevice(this.Id, userId, readerId, number));
        }

        public void UpdateFromDeviceInfo(string userId, int type, string deviceVersion)
        {
            this.ApplyChange(new DeviceUpdatedFromDeviceInfo(this.Id, userId, type, deviceVersion));
        }

        public void Apply(DeviceCreated e)
        {
            this.Id = e.AggregateId;
            ////this.UserId = e.UserId;
            this.Name = e.Name;
            this.IpAddress = e.IpAddress;
            this.DeviceType = e.DeviceType;
            this.ModBusId = e.ModBusId;
            ////this.Mode = e.Mode;
            this.IsCheckInOrOut = e.IsCheckInOrOut;
            this.ExternalId = e.ExternalId;
            this.MacAddress = e.MacAddress;
            this.AccessType = e.AccessType;
            this.LocationName = e.LocationName;
            this.HasMaglock = e.HasMaglock;
        }
        
        public void Apply(ReaderAddedToDevice e)
        {
            ////this.UserId = e.UserId;

            this.Readers.Add(new Reader(e.ReaderId, e.Number));
        }

        public void Apply(DeviceUpdatedFromDeviceInfo e)
        {
            ////this.UserId = e.UserId;

            this.Type = e.Type;
            this.DeviceVersion = e.DeviceVersion;
        }
    }
}