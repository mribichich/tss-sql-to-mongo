namespace TssSqlToMongo.WriteModel.Handlers
{
    using System.Diagnostics;

    using TssSqlToMongo.AggregateRoots;
    using TssSqlToMongo.Core;
    using TssSqlToMongo.WriteModel.Commands;

    public class UpdateDeviceFromDeviceInfoHandler : ICommandHandler<UpdateDeviceFromDeviceInfoCommand>
    {
        private readonly ISession session;

        public UpdateDeviceFromDeviceInfoHandler(
            ISession session)
        {
            this.session = session;
        }

        public void Handle(UpdateDeviceFromDeviceInfoCommand message)
        {
            Debug.WriteLine($"{nameof(message)}: {message.Id} / {message.ExpectedVersion}");
            
            var device = this.session.Get<Device>(message.Id, message.ExpectedVersion);

            device.UpdateFromDeviceInfo(
                message.UserId,
                message.Type,
                message.DeviceVersion);
        }
    }
}