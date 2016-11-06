namespace ConsoleApplication2.ReadModel.Events
{
    using System;

    using ConsoleApplication2.Core;

    public abstract class BaseDevicesEvent : BaseEvent
    {
        protected BaseDevicesEvent(Guid aggregateId, string userId)
            : base(aggregateId, userId)
        {
            this.EntityType = "Device";
        }
    }
}