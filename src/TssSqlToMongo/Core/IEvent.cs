namespace ConsoleApplication2.Core
{
    using System;

    public interface IEvent : IMessage
    {
        Guid Id { get; }

        Guid AggregateId { get; set; }

        int Version { get; set; }

        DateTime Timestamp { get; set; }

        string UserId { get; set; }

        string EntityType { get; }
    }
}