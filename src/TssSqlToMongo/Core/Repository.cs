namespace ConsoleApplication2.Core
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class Repository : IRepository
    {
        private readonly IEventStore eventStore;
        private readonly IEventPublisher publisher;

        public Repository(IEventStore eventStore, IEventPublisher publisher)
        {
            if (eventStore == null)
            {
                throw new ArgumentNullException(nameof(eventStore));
            }

            if (publisher == null)
            {
                throw new ArgumentNullException(nameof(publisher));
            }

            this.eventStore = eventStore;
            this.publisher = publisher;
        }

        public void Save<T>(T aggregate, int? expectedVersion = null)
            where T : AggregateRoot
        {
            if (expectedVersion != null && this.eventStore.Get(aggregate.Id, expectedVersion.Value)
                                               .Any())
            {
                throw new ConcurrencyException(aggregate.Id);
            }

            var changes = aggregate.FlushUncommitedChanges().ToList();

            this.eventStore.Save(changes);

            foreach (var @event in changes)
            {
                this.publisher.Publish(@event);
            }
        }

        public async Task SaveAsync<T>(T aggregate, int? expectedVersion = null)
            where T : AggregateRoot
        {
            if (expectedVersion != null && this.eventStore.Get(aggregate.Id, expectedVersion.Value)
                                               .Any())
            {
                throw new ConcurrencyException(aggregate.Id);
            }

            var changes = aggregate.FlushUncommitedChanges().ToList();

            this.eventStore.Save(changes);

            foreach (var @event in changes)
            {
                this.publisher.Publish(@event);
            }
        }

        public T Get<T>(Guid aggregateId)
            where T : AggregateRoot
        {
            return this.LoadAggregate<T>(aggregateId);
        }

        private T LoadAggregate<T>(Guid id)
            where T : AggregateRoot
        {
            var aggregate = AggregateFactory.CreateAggregate<T>();

            var events = this.eventStore.Get(id, -1).ToList();

            if (!events.Any())
            {
                throw new AggregateNotFoundException(id);
            }

            aggregate.LoadFromHistory(events);
            return aggregate;
        }
    }
}