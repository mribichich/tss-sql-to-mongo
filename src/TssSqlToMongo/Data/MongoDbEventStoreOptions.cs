namespace TssSqlToMongo.Data
{
    public class MongoDbEventStoreOptions
    {
        private const string DefaultHost = "localhost";
        private const int DefaultPort = 27017;
        private const string DefaultDatabase = "SisControlPanelApi";
        private const string DefaultCollection = "events";

        public string Host { get; set; } = DefaultHost;

        public int? Port { get; set; } = DefaultPort;

        public string Database { get; set; } = DefaultDatabase;

        public string Collection { get; set; } = DefaultCollection;

        public MongoDbEventStoreOptions UseHost(string host)
        {
            this.Host = host ?? DefaultHost;

            return this;
        }

        public MongoDbEventStoreOptions UsePort(int? port)
        {
            this.Port = port ?? DefaultPort;

            return this;
        }

        public MongoDbEventStoreOptions UseDatabase(string database)
        {
            this.Database = database ?? DefaultDatabase;

            return this;
        }

        public MongoDbEventStoreOptions UseCollection(string collection)
        {
            this.Collection = collection ?? DefaultCollection;

            return this;
        }
    }
}