namespace TssSqlToMongo.Config
{
    public class MongoDbEventStoreConfig
    {
        public string Host { get; set; }

        public int? Port { get; set; }

        public string Database { get; set; }

        public string Collection { get; set; }
    }
}