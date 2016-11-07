namespace TssSqlToMongo.Core
{
    using System;
    using System.Threading.Tasks;

    public interface IRepository
    {
        T Get<T>(Guid aggregateId)
            where T : AggregateRoot;

        void Save<T>(T aggregate, int? expectedVersion = null)
            where T : AggregateRoot;

        Task SaveAsync<T>(T aggregate, int? expectedVersion = null)
            where T : AggregateRoot;
    }
}