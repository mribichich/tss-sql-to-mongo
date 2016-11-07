namespace TssSqlToMongo.Core
{
    using System;
    using System.Collections.Generic;

    public interface IEventStore
    {
        void Save(IEnumerable<IEvent> events);

        IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion);
    }
}