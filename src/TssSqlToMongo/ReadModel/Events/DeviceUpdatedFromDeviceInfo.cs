namespace TssSqlToMongo.ReadModel.Events
{
    using System;

    public class DeviceUpdatedFromDeviceInfo : BaseDevicesEvent
    {
        public DeviceUpdatedFromDeviceInfo(Guid aggregateId, string userId, int type, string deviceVersion)
            : base(aggregateId, userId)
        {
            this.Type = type;
            this.DeviceVersion = deviceVersion;
        }

        public int Type { get; protected set; }

        public string DeviceVersion { get; protected set; }
    }
}