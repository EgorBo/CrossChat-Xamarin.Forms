using Crosschat.Client.Model.Contracts;
using Crosschat.Client.Model.Proxies;
using Crosschat.Server.Application.DataTransferObjects.Utils;
using Crosschat.Server.Infrastructure.Protocol;

namespace Crosschat.Client.Model.Managers
{
    /// <summary>
    /// Managers locator
    /// </summary>
    public class ApplicationManager
    {        
        public ApplicationManager(
            ITransportResource transportResource,
            IDtoSerializer dtoSerializer,
            IStorage storage,
            IDeviceInfo deviceInfo)
        {
            //we don't have autofac so let's build the tree 
            var commandParser = new CommandParser();
            var connectionManager = new ConnectionManager(transportResource, new CommandBuffer(commandParser), commandParser, new RequestsHandler(), dtoSerializer);
            
            AccountManager = new AccountManager(storage, deviceInfo, connectionManager, 
                new ProfileServiceProxy(connectionManager), 
                new RegistrationServiceProxy(connectionManager), 
                new AuthenticationServiceProxy(connectionManager));
            ChatManager = new ChatManager(connectionManager, new ChatServiceProxy(connectionManager), AccountManager);
            FriendsManager = new FriendsManager(connectionManager, new FriendsServiceProxy(connectionManager));
            SearchManager = new SearchManager(connectionManager, new UsersSearchServiceProxy(connectionManager));
            ConnectionManager = connectionManager;
        }

        public ConnectionManager ConnectionManager { get; private set; }

        public AccountManager AccountManager { get; private set; }

        public ChatManager ChatManager { get; private set; }

        public FriendsManager FriendsManager { get; private set; }

        public SearchManager SearchManager { get; private set; }
    }
}
