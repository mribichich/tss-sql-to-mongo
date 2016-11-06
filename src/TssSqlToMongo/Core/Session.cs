namespace ConsoleApplication2.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Session : ISession
    {
        private readonly IRepository repository;
        private readonly Dictionary<Guid, AggregateDescriptor> trackedAggregates;

        public Session(IRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            this.repository = repository;
            this.trackedAggregates = new Dictionary<Guid, AggregateDescriptor>();
        }

        public void Add<T>(T aggregate)
            where T : AggregateRoot
        {
            if (!this.IsTracked(aggregate.Id))
            {
                this.trackedAggregates.Add(aggregate.Id, new AggregateDescriptor { Aggregate = aggregate, Version = aggregate.Version });
            }
            else if (this.trackedAggregates[aggregate.Id].Aggregate != aggregate)
            {
                throw new ConcurrencyException(aggregate.Id);
            }
        }

        public T Get<T>(Guid id, int? expectedVersion = null)
            where T : AggregateRoot
        {
            if (this.IsTracked(id))
            {
                var trackedAggregate = (T)this.trackedAggregates[id].Aggregate;

                if (expectedVersion != null && trackedAggregate.Version != expectedVersion)
                {
                    throw new ConcurrencyException(trackedAggregate.Id);
                }

                return trackedAggregate;
            }

            var aggregate = this.repository.Get<T>(id);

            if (expectedVersion != null && aggregate.Version != expectedVersion)
            {
                throw new ConcurrencyException(id);
            }

            this.Add(aggregate);

            return aggregate;
        }

        public void Commit()
        {
            foreach (var descriptor in this.trackedAggregates.Values)
            {
                this.repository.Save(descriptor.Aggregate, descriptor.Version);
            }

            this.trackedAggregates.Clear();
        }

        public async Task CommitAsync()
        {
            foreach (var descriptor in this.trackedAggregates.Values)
            {
                await this.repository.SaveAsync(descriptor.Aggregate, descriptor.Version);
            }

            this.trackedAggregates.Clear();
        }

        private bool IsTracked(Guid id)
        {
            return this.trackedAggregates.ContainsKey(id);
        }

        private class AggregateDescriptor
        {
            public AggregateRoot Aggregate { get; set; }

            public int Version { get; set; }
        }
    }
}