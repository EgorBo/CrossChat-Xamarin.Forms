using System.Data.Entity;
using System.Linq;
using Crosschat.Server.Application.Contracts;
using Crosschat.Server.Infrastructure.Persistence.EF;
using Crosschat.Server.Infrastructure.Transport;
using Crosschat.Utils.Logging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;

namespace Crosschat.Server.Infrastructure
{
    public class InfrastructureInitializator : IInfrastructureInitializator
    {
        private readonly ISettings _settings;
        private readonly CrosschatSocketServer _crosschatSocketServer;
        private static readonly ILogger Logger = LogFactory.GetLogger<InfrastructureInitializator>();

        public InfrastructureInitializator(ISettings settings, CrosschatSocketServer crosschatSocketServer)
        {
            _settings = settings;
            _crosschatSocketServer = crosschatSocketServer;
        }
        
        public async void Init()
        {
            Logger.Info("Initing...");
            var config = new ServerConfig
                {
                    Port = _settings.ServerPort,
                    Ip = "Any", 
                    MaxConnectionNumber = 2000,
                    Mode = SocketMode.Tcp,
                    //ReceiveBufferSize = 1024,
                    //SendBufferSize = 1024,
                    Name = "CrosschatSocketServer",
                    DisableSessionSnapshot = true,
                    LogAllSocketException = false,
                    LogBasicSessionActivity = false,
                    LogCommand = false, 
                    //LogFactory = "DefaultLogFactory"
                };

            var setuped = _crosschatSocketServer.Setup(config);
            var started = _crosschatSocketServer.Start();
            //Database.SetInitializer(new DropCreateDatabaseAlways<UnitOfWork>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<UnitOfWork>());
            var player = new UnitOfWork().Users.FirstOrDefault();
            Logger.Info("Init completed {0}({1})", setuped, started);
        }
    }
}
