namespace TssSqlToMongo.ReadModel.Events
{
    using System;

    using TssSqlToMongo.Core;

    public abstract class BaseDevicesEvent : BaseEvent
    {
        protected BaseDevicesEvent(Guid aggregateId, string userId)
            : base(aggregateId, userId)
        {
            this.EntityType = "Device";
        }
    }
}