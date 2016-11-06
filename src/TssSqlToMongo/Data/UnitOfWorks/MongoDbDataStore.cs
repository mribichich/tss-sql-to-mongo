namespace ConsoleApplication2.Data.UnitOfWorks
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using ConsoleApplication2.Core;
    using ConsoleApplication2.Data.Entities;

    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Driver;

    public class MongoDbDataStore : IDataStore
    {
        private readonly MongoDbDataStoreOptions options;
        private IMongoDatabase mongoDatabase;

       public MongoDbDataStore(MongoDbDataStoreOptions options)
        {
            this.options = options;

            this.Config();
        }
        
        public void Insert<T>(T entity) where T : IDbEntity
        {
            var entityCollectionName = this.MapEntityToCollection(typeof(T));
            
              this.mongoDatabase.GetCollection<T>(entityCollectionName)
              .InsertOneAsync(entity);
        }

        public void FindOneAndUpdate<T>(Expression<Func<T, bool>> filter, UpdateDefinition<T> updateDefinition) where T : IDbEntity
        {
            var entityCollectionName = this.MapEntityToCollection(typeof(T));

            this.mongoDatabase.GetCollection<T>(entityCollectionName)
                .FindOneAndUpdate(filter, updateDefinition);
        }
        
        private string MapEntityToCollection(Type type)
        {
            //// todo: should be extracted and inject by configuration or DI service
            
            if (type == typeof(DeviceDb))
            {
                return "devices";
            }

            throw new ArgumentOutOfRangeException(nameof(type), "Entity collection does not exist");
        }

        private void Config()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
            
            BsonClassMap.RegisterClassMap<DeviceDb>();
            BsonClassMap.RegisterClassMap<ReaderDb>();

            var client = new MongoClient($"mongodb://{this.options.Host}:{this.options.Port}");
            this.mongoDatabase = client.GetDatabase(this.options.Database);
        }
    }
}