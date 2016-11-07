namespace TssSqlToMongo.Data.UnitOfWorks
{
    public class MongoDbDataStoreOptions
    {
        private const string DefaultHost = "localhost";
        private const int DefaultPort = 27017;
        private const string DefaultDatabase = "SisControlPanelApi";

        public string Host { get; set; } = DefaultHost;

        public int? Port { get; set; } = DefaultPort;

        public string Database { get; set; } = DefaultDatabase;

        public MongoDbDataStoreOptions UseHost(string host)
        {
            this.Host = host ?? DefaultHost;

            return this;
        }

        public MongoDbDataStoreOptions UsePort(int? port)
        {
            this.Port = port ?? DefaultPort;

            return this;
        }

        public MongoDbDataStoreOptions UseDatabase(string database)
        {
            this.Database = database ?? DefaultDatabase;

            return this;
        }
    }
}