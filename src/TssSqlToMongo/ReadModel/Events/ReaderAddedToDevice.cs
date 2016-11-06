namespace ConsoleApplication2.ReadModel.Events
{
    using System;

    public class ReaderAddedToDevice : BaseDevicesEvent
    {
        public ReaderAddedToDevice(Guid aggregateId, string userId, Guid readerId, int number)
            : base(aggregateId, userId)
        {
            this.ReaderId = readerId;
            this.Number = number;
        }

        public Guid ReaderId { get; protected set; }

        public int Number { get; protected set; }
    }
}