namespace ConsoleApplication2.WriteModel.Handlers
{
    using System.Diagnostics;

    using ConsoleApplication2.AggregateRoots;
    using ConsoleApplication2.Core;
    using ConsoleApplication2.WriteModel.Commands;

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