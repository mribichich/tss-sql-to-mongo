﻿namespace ConsoleApplication2.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ConsoleApplication2.Core;
    using ConsoleApplication2.ReadModel.Events;

    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Driver;

    public class MongoDbEventStore : IEventStore
    {
        private readonly MongoDbEventStoreOptions options;
        private IMongoCollection<BaseEvent> mongoCollection;

        public MongoDbEventStore(MongoDbEventStoreOptions options)
        {
            this.options = options;

            this.Config();
        }

        public void Save(IEnumerable<IEvent> events)
        {
            this.mongoCollection.InsertManyAsync(events.Cast<BaseEvent>());
        }

        public IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion)
        {
            return  this.mongoCollection.Find(f => f.AggregateId == aggregateId && f.Version > fromVersion).ToList();
        }

        private void Config()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            this.RegisterAllEvents();

            var client = new MongoClient($"mongodb://{this.options.Host}:{this.options.Port}");
            var database = client.GetDatabase(this.options.Database);
            this.mongoCollection = database.GetCollection<BaseEvent>(this.options.Collection);
        }

        private void RegisterAllEvents()
        {
            BsonClassMap.RegisterClassMap<DeviceCreated>();
            BsonClassMap.RegisterClassMap<ReaderAddedToDevice>();
            BsonClassMap.RegisterClassMap<DeviceUpdatedFromDeviceInfo>();
        }
    }
}