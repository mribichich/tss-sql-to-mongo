using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    using System;
    using System.Data.SqlClient;
    using System.Reflection;

    using Autofac;

    using ConsoleApplication2.Core;
    using ConsoleApplication2.Data;
    using ConsoleApplication2.Data.UnitOfWorks;
    using ConsoleApplication2.ReadModel.Services;
    using ConsoleApplication2.Sql;
    using ConsoleApplication2.ValueObjects;
    using ConsoleApplication2.WriteModel.Commands;
    using ConsoleApplication2.WriteModel.Handlers;

    class Program
    {
        private static List<DeviceSql> controllers;
        private static List<ReaderSql> readers;
        private static IContainer container;

        static void Main(string[] args)
        {
            UseAutoFac();

            GetSqlData();

            try
            {
                var commandSender = container.Resolve<ICommandSender>();

                for (var i = 0; i < 2; i++)
                {
                    Console.Write("creating device...");

                    var device = controllers[i];

                    var cmd = new CreateDeviceCommand(
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
                        device.HasMaglock);

                    commandSender.Send(cmd);

                    Console.WriteLine(" done!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Done!");

            Console.Read();
        }

        private static void UseAutoFac()
        {
            var builder = new ContainerBuilder();

            builder.Register(c => new MongoDbDataStoreOptions().UseDatabase("SisControlPanelApi2"));
            builder.RegisterType<MongoDbDataStore>()
                .As<IDataStore>()
                .SingleInstance();
            builder.Register(c => new MongoDbEventStoreOptions().UseDatabase("SisControlPanelApi2"));
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
            builder.RegisterType<Repository>()
                .As<IRepository>();

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

        private static void GetSqlData()
        {
            SqlConnection sqlConn = null;

            try
            {
                sqlConn =
                    new SqlConnection(
                        @"Data Source=.\SQLEXPRESS;Initial Catalog=Sis.Access_dev;user id=sisserver;password=aM6T30jFDM3VbwU5;MultipleActiveResultSets=True");

                sqlConn.Open();

                controllers = GetControllers(sqlConn);

                readers = GetReaders(sqlConn);

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
            catch (Exception e)
            {
                Console.WriteLine(e);
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

            var controllers = new List<DeviceSql>();

            while (dataReader.Read())
            {
                var controller = new DeviceSql()
                    {
                        Id = Guid.Parse(dataReader["ID_Controladora"].ToString()),
                        Name = dataReader["Name"]?.ToString(),
                        IpAddress = dataReader["IpAddress"]?.ToString(),
                        DeviceType = dataReader["DeviceType"]?.ToString(),
                        ModBusId = Convert.ToInt32(dataReader["ID_Modbus"]),
                        IsCheckInOrOut = Convert.ToBoolean(dataReader["IsCheckInOrOut"]),
                        ExternalId = Convert.ToInt32(dataReader["id"]),
                        MacAddress = dataReader["MacAddress"]?.ToString(),
                        DeviceVersion = dataReader["DeviceVersion"]?.ToString(),
                        Type = dataReader["Type"] != DBNull.Value ? Convert.ToInt32(dataReader["Type"]) : (int?)null,
                        AccessType = dataReader["AccessType"] != DBNull.Value ? Convert.ToInt32(dataReader["AccessType"]) : (int?)null,
                        LocationName = dataReader["LocationName"]?.ToString(),
                        HasMaglock = dataReader["HasMaglock"] != DBNull.Value ? Convert.ToBoolean(dataReader["HasMaglock"]) : (bool?)null
                    };

                controllers.Add(controller);
            }

            return controllers;
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
    }
}
