namespace ConsoleApplication2.WriteModel.Commands
{
    using System;

    using ConsoleApplication2.Core;

    public class UpdateDeviceFromDeviceInfoCommand : BaseCommand
    {
        public UpdateDeviceFromDeviceInfoCommand(
            Guid id,
            int? expectedVersion,
            string userId,
            int type,
            string deviceVersion)
            : base(id, expectedVersion, userId)
        {
            this.Type = type;
            this.DeviceVersion = deviceVersion;
        }

        public int Type { get; }
   
        public string DeviceVersion { get; }
 }
}