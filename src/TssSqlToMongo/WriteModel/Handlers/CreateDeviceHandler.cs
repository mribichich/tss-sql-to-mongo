namespace ConsoleApplication2.WriteModel.Handlers
{
    using System.Diagnostics;

    using ConsoleApplication2.AggregateRoots;
    using ConsoleApplication2.Core;
    using ConsoleApplication2.WriteModel.Commands;

    public class CreateDeviceHandler : ICommandHandler<CreateDeviceCommand>
    {
        private readonly ISession session;

        public CreateDeviceHandler(
            ISession session)
        {
            this.session = session;
        }

        public void Handle(CreateDeviceCommand message)
        {
            Debug.WriteLine($"{nameof(message)}: {message.Id} / {message.ExpectedVersion}");
            
            var device = new Device(
                message.Id,
                message.UserId,
                message.Name,
                message.IpAddress,
                message.DeviceType,
                message.ModBusId,
                ////message.Mode,
                message.IsCheckInOrOut,
                message.ExternalId,
                message.MacAddress,
                message.AccessType,
                message.LocationName,
                message.HasMaglock);

            foreach (var reader in message.Readers)
            {
                 device.AddReader(message.UserId, reader.Id, reader.Number);
            }
            
           this.session.Add(device);
        }
    }
}