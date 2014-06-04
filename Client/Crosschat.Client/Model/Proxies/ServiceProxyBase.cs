namespace Crosschat.Client.Model.Proxies
{

    public class ServiceProxyBase
    {
        protected ConnectionManager ConnectionManager { get; private set; }

        public ServiceProxyBase(ConnectionManager connectionManager)
        {
            ConnectionManager = connectionManager;
        }
    }
}
