namespace ConsoleApplication2.Core
{
    using System;

    public class BaseEvent : IEvent
    {
        public BaseEvent(Guid aggregateId, string userId)
        {
            this.AggregateId = aggregateId;
            this.UserId = userId;
        }

        public Guid Id { get; protected set; } = Guid.NewGuid();

        public Guid AggregateId { get; set; }

        public int Version { get; set; }

        public DateTime Timestamp { get; set; }

        public string UserId { get; set; }

        public string EntityType { get; protected set; }
    }
}