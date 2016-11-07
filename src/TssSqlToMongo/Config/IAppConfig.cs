namespace TssSqlToMongo.Config
{
    public interface IAppConfig
    {
        SisAccessSqlDbConfig SisAccessSqlDb { get; }

        MongoDbEventStoreConfig MongoDbEventStore { get; }

        MongoDbDataStoreConfig MongoDbDataStore { get; }
    }
}