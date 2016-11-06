namespace ConsoleApplication2.Core
{
    using System;
    using System.Linq.Expressions;

    using ConsoleApplication2.Data.Entities;

    using MongoDB.Driver;

    public interface IDataStore
    {
        void Insert<T>(T entity) where T : IDbEntity;
        
        void FindOneAndUpdate<T>(Expression<Func<T, bool>> filter, UpdateDefinition<T> updateDefinition)
            where T : IDbEntity;
    }
}