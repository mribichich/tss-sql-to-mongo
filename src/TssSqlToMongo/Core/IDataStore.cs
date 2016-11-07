namespace TssSqlToMongo.Core
{
    using System;
    using System.Linq.Expressions;

    using MongoDB.Driver;

    using TssSqlToMongo.Data.Entities;

    public interface IDataStore
    {
        void Insert<T>(T entity) where T : IDbEntity;
        
        void FindOneAndUpdate<T>(Expression<Func<T, bool>> filter, UpdateDefinition<T> updateDefinition)
            where T : IDbEntity;
    }
}