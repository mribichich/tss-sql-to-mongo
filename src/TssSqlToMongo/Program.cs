namespace TssSqlToMongo
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Autofac;

    using TssSqlToMongo.Config;
    using TssSqlToMongo.Core;
    using TssSqlToMongo.Data;
    using TssSqlToMongo.Data.UnitOfWorks;
    using TssSqlToMongo.ReadModel.Services;
    using TssSqlToMongo.Sql;
    using TssSqlToMongo.ValueObjects;
    using TssSqlToMongo.WriteModel.Commands;
    using TssSqlToMongo.WriteModel.Handlers;

    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    class Program
    {
        private static List<DeviceSql> controllers;
        private static IContainer container;

        static void Main(string[] args)
        {
            try
            {
                UseAutoFac();

                var appConfig = container.Resolve<IAppConfig>();

                GetSqlData(appConfig);

                SaveToMongoDb();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("\nDone!");

            Console.Read();
        }

        private static void UseAutoFac()
        {
            var config = AddConfig();

            var builder = new ContainerBuilder();

            builder.RegisterInstance(config)
                .As<IAppConfig>()
                .SingleInstance();

            builder.Register(
                c => new MongoDbDataStoreOptions().UseHost(config.MongoDbDataStore?.Host)
                         .UsePort(config.MongoDbDataStore?.Port)
                         .UseDatabase(config.MongoDbDataStore?.Database));

            builder.RegisterType<MongoDbDataStore>()
                .As<IDataStore>()
                .SingleInstance();

            builder.Register(
                c => new MongoDbEventStoreOptions().UseHost(config.MongoDbEventStore?.Host)
                         .UsePort(config.MongoDbEventStore?.Port)
                         .UseDatabase(config.MongoDbEventStore?.Database)
                         .UseCollection(config.MongoDbEventStore?.Collection));

            builder.RegisterType<MongoDbEventStore>()
                .As<IEventStore>()
                .SingleInstance();

            builder.RegisterType<Session>()
                .As<ISession>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Translator") || t.Name.EndsWith("Handler"))
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<DevicesWriteService>()
                .As<IDevicesWriteService>();

            builder.RegisterType<Repository>();

            builder.Register(c => new CacheRepository(c.Resolve<Repository>(), c.Resolve<IEventStore>()))
                .As<IRepository>()
                .SingleInstance();

            builder.RegisterType<InProcessBus>()
                .As<ICommandSender>()
                .As<IHandlerRegistrar>()
                .As<IEventPublisher>()
                .SingleInstance();

            builder.RegisterGeneric(typeof(TransactionalHandler<>));

            container = builder.Build();

            var registrar = new BusRegistrar(container);
            registrar.Register(typeof(CreateDeviceHandler));
        }

        private static IAppConfig AddConfig()
        {
            var config = new AppConfig();

            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"conf\appsettings.yaml");

            if (File.Exists(configFilePath))
            {
                var content = File.ReadAllText(configFilePath);

                var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
                config = deserializer.Deserialize<AppConfig>(content);
            }

            return config;
        }

        private static void GetSqlData(IAppConfig appConfig)
        {
            SqlConnection sqlConn = null;

            try
            {
                var builder = new SqlConnectionStringBuilder()
                {
                    DataSource = appConfig.SisAccessSqlDb.Host,
                    InitialCatalog = appConfig.SisAccessSqlDb.Database,
                    UserID = appConfig.SisAccessSqlDb.Username,
                    Password = appConfig.SisAccessSqlDb.Password,
                    MultipleActiveResultSets = true
                };

                sqlConn = new SqlConnection(builder.ConnectionString);

                sqlConn.Open();

                controllers = GetControllers(sqlConn);

                var readers = GetReaders(sqlConn);

                foreach (var controller in controllers)
                {
                    var controllerReaders = readers.Where(w => w.ControllerId == controller.Id)
                        .ToList();

                    if (controllerReaders.Count != 2)
                    {
                        throw new Exception("Controller readers count different than 2");
                    }

                    controller.Readers = new List<ReaderSql>(controllerReaders);
                }
            }
            finally
            {
                sqlConn?.Close();
            }
        }

        private static List<DeviceSql> GetControllers(SqlConnection sqlConn)
        {
            var sqlComm = new SqlCommand("SELECT * FROM Controladoras", sqlConn);

            var dataReader = sqlComm.ExecuteReader();

            var deviceSqls = new List<DeviceSql>();

            while (dataReader.Read())
            {
                var controller = new DeviceSql()
                {
                    Id = Guid.Parse(dataReader["ID_Controladora"].ToString()),
                    Name = dataReader["Name"].ToString(),
                    IpAddress = dataReader["IpAddress"].ToString(),
                    DeviceType = dataReader["DeviceType"] != DBNull.Value ? dataReader["DeviceType"].ToString() : null,
                    ModBusId = Convert.ToInt32(dataReader["ID_Modbus"]),
                    IsCheckInOrOut = Convert.ToBoolean(dataReader["IsCheckInOrOut"]),
                    ExternalId = Convert.ToInt32(dataReader["id"]),
                    MacAddress = dataReader["MacAddress"] != DBNull.Value ? dataReader["MacAddress"].ToString() : null,
                    DeviceVersion = dataReader["DeviceVersion"] != DBNull.Value ? dataReader["DeviceVersion"].ToString() : null,
                    Type = dataReader["Type"] != DBNull.Value ? Convert.ToInt32(dataReader["Type"]) : (int?)null,
                    AccessType = dataReader["AccessType"] != DBNull.Value ? Convert.ToInt32(dataReader["AccessType"]) : (int?)null,
                    LocationName = dataReader["LocationName"] != DBNull.Value ? dataReader["LocationName"].ToString() : null,
                    HasMaglock = dataReader["HasMaglock"] != DBNull.Value ? Convert.ToBoolean(dataReader["HasMaglock"]) : (bool?)null
                };

                deviceSqls.Add(controller);
            }

            return deviceSqls;
        }

        private static List<ReaderSql> GetReaders(SqlConnection sqlConn)
        {
            var sqlComm = new SqlCommand("SELECT * FROM Readers", sqlConn);

            var dataReader = sqlComm.ExecuteReader();

            var readers = new List<ReaderSql>();

            while (dataReader.Read())
            {
                var reader = new ReaderSql()
                {
                    Id = Guid.Parse(dataReader["Id"].ToString()),
                    Number = Convert.ToInt32(dataReader["Number"]),
                    ControllerId = Guid.Parse(dataReader["ControllerId"].ToString()),
                };

                readers.Add(reader);
            }

            return readers;
        }

        private static void SaveToMongoDb()
        {
            var commandSender = container.Resolve<ICommandSender>();

            foreach (var device in controllers)
            {
                Console.Write("creating devices: ");

                commandSender.Send(
                    new CreateDeviceCommand(
                        device.Id,
                        null,
                        device.Name,
                        device.IpAddress,
                        device.DeviceType,
                        device.ModBusId,
                        device.IsCheckInOrOut,
                        device.ExternalId,
                        device.MacAddress,
                        device.Readers.Select(s => new Reader(s.Id, s.Number)),
                        device.AccessType,
                        device.LocationName,
                        device.HasMaglock));

                if (device.Type.HasValue)
                {
                    commandSender.Send(new UpdateDeviceFromDeviceInfoCommand(device.Id, null, null, device.Type.Value, device.DeviceVersion));
                }

                Console.Write(".");
            }
        }
    }
}
