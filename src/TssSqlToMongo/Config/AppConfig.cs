namespace TssSqlToMongo.Config
{
    public class AppConfig : IAppConfig
    {
        public SisAccessSqlDbConfig SisAccessSqlDb { get; set; }

        public MongoDbEventStoreConfig MongoDbEventStore { get; set; }

        public MongoDbDataStoreConfig MongoDbDataStore { get; set; }
    }
}