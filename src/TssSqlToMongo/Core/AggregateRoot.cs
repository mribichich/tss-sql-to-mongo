namespace TssSqlToMongo.Core
{
    using System;
    using System.Collections.Generic;

    public abstract class AggregateRoot
    {
        private readonly List<IEvent> changes = new List<IEvent>();

        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            lock (this.changes)
            {
                return this.changes.ToArray();
            }
        }

        public IEnumerable<IEvent> FlushUncommitedChanges()
        {
            lock (this.changes)
            {
                var changes = this.changes.ToArray();
                var i = 0;

                foreach (var @event in changes)
                {
                    if (@event.AggregateId == Guid.Empty && this.Id == Guid.Empty)
                    {
                        throw new AggregateOrEventMissingIdException(this.GetType(), @event.GetType());
                    }

                    if (@event.AggregateId == Guid.Empty)
                    {
                        @event.AggregateId = this.Id;
                    }

                    i++;

                    @event.Version = this.Version + i;
                    @event.Timestamp = DateTime.UtcNow;
                }

                this.Version = this.Version + this.changes.Count;
                this.changes.Clear();

                return changes;
            }
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                if (e.Version != this.Version + 1)
                {
                    throw new EventsOutOfOrderException(e.AggregateId);
                }

                this.ApplyChange(e, false);
            }
        }

        protected void ApplyChange(IEvent @event)
        {
            this.ApplyChange(@event, true);
        }

        private void ApplyChange(IEvent @event, bool isNew)
        {
            lock (this.changes)
            {
                this.AsDynamic().Apply(@event);

                if (isNew)
                {
                    this.changes.Add(@event);
                }
                else
                {
                    this.Id = @event.AggregateId;
                    this.Version++;
                }
            }
        }
    }
}