namespace TssSqlToMongo.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Threading.Tasks;

    public class CacheRepository : IRepository
    {
        private static readonly ConcurrentDictionary<string, object> Locks = new ConcurrentDictionary<string, object>();
        private readonly IRepository repository;
        private readonly IEventStore eventStore;
        private readonly MemoryCache memoryCache;
        private readonly Func<CacheItemPolicy> policyFactory;

        public CacheRepository(IRepository repository, IEventStore eventStore)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (eventStore == null)
            {
                throw new ArgumentNullException(nameof(eventStore));
            }

            this.repository = repository;
            this.eventStore = eventStore;
            this.memoryCache = MemoryCache.Default;
            this.policyFactory = () => new CacheItemPolicy
            {
                SlidingExpiration = new TimeSpan(0, 0, 15, 0),
                RemovedCallback = x =>
                {
                    object o;
                    Locks.TryRemove(x.CacheItem.Key, out o);
                }
            };
        }

        public void Save<T>(T aggregate, int? expectedVersion = null)
            where T : AggregateRoot
        {
            var idstring = aggregate.Id.ToString();

            try
            {
                lock (Locks.GetOrAdd(idstring, _ => new object()))
                {
                    if (aggregate.Id != Guid.Empty && !this.IsTracked(aggregate.Id))
                    {
                        this.memoryCache.Add(idstring, aggregate, this.policyFactory.Invoke());
                    }

                    this.repository.Save(aggregate, expectedVersion);
                }
            }
            catch (Exception)
            {
                lock (Locks.GetOrAdd(idstring, _ => new object()))
                {
                    this.memoryCache.Remove(idstring);
                }

                throw;
            }
        }

        public async Task SaveAsync<T>(T aggregate, int? expectedVersion = null)
            where T : AggregateRoot
        {
            var idstring = aggregate.Id.ToString();

            try
            {
                lock (Locks.GetOrAdd(idstring, _ => new object()))
                {
                    if (aggregate.Id != Guid.Empty && !this.IsTracked(aggregate.Id))
                    {
                        this.memoryCache.Add(idstring, aggregate, this.policyFactory.Invoke());
                    }

                    this.repository.Save(aggregate, expectedVersion);
                }
            }
            catch (Exception)
            {
                lock (Locks.GetOrAdd(idstring, _ => new object()))
                {
                    this.memoryCache.Remove(idstring);
                }

                throw;
            }
        }

        public T Get<T>(Guid aggregateId)
            where T : AggregateRoot
        {
            var idstring = aggregateId.ToString();

            try
            {
                lock (Locks.GetOrAdd(idstring, _ => new object()))
                {
                    T aggregate;

                    if (this.IsTracked(aggregateId))
                    {
                        aggregate = (T)this.memoryCache.Get(idstring);

                        var events = this.eventStore.Get(aggregateId, aggregate.Version).ToList();

                        if (events.Any() && events.First().Version != aggregate.Version + 1)
                        {
                            this.memoryCache.Remove(idstring);
                        }
                        else
                        {
                            aggregate.LoadFromHistory(events);
                            return aggregate;
                        }
                    }

                    aggregate = this.repository.Get<T>(aggregateId);

                    this.memoryCache.Add(aggregateId.ToString(), aggregate, this.policyFactory.Invoke());

                    return aggregate;
                }
            }
            catch (Exception)
            {
                lock (Locks.GetOrAdd(idstring, _ => new object()))
                {
                    this.memoryCache.Remove(idstring);
                }

                throw;
            }
        }

        private bool IsTracked(Guid id)
        {
            return this.memoryCache.Contains(id.ToString());
        }
    }
}