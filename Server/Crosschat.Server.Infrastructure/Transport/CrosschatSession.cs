using System;
using System.Threading;
using System.Threading.Tasks;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Server.Application.DataTransferObjects.Utils;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Application.Sessions;
using Crosschat.Server.Domain.Entities;
using Crosschat.Server.Infrastructure.Protocol;
using Crosschat.Utils.Logging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Crosschat.Server.Infrastructure.Transport
{
    public class CrosschatSession : AppSession<CrosschatSession, BinaryRequestInfo>, ISession
    {
        private new static readonly ILogger Logger = LogFactory.GetLogger<CrosschatSession>();
        private readonly IDtoSerializer _serializer = ServiceLocator.Resolve<IDtoSerializer>();
        private readonly CommandParser _commandParser = ServiceLocator.Resolve<CommandParser>();
        private readonly RequestsHandler _requestsHandler = ServiceLocator.Resolve<RequestsHandler>();
        private readonly TransportPerformanceMeasurer _performanceMeasurer = ServiceLocator.Resolve<TransportPerformanceMeasurer>();
        private long _lastToken = 1;
        private bool _isOpen;

        public CrosschatSession()
        {
        }

        protected override void OnSessionStarted()
        {
            Logger.Debug("started {0}", GetSessionName());
            IsOpen = true;
            base.OnSessionStarted();
        }

        internal string GetSessionName()
        {
            return (IsAuthorized ? User.Name : "__not_auth") + "[" + SessionID.Substring(0, 5) + "]";
        }

        internal void AppendResponse(byte[] responseData)
        {
            try
            {
                var response = _serializer.Deserialize<ResponseBase>(responseData);
                if (response != null)
                    _requestsHandler.AppendResponse(response);
            }
            catch (Exception exc)
            {
                Logger.Exception(exc, "Tried to recognize {0} bytes as request for {1}", responseData == null ? "NULL" : responseData.Length.ToString(), GetSessionName());
            }
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            Logger.Debug("closed ({0})", GetSessionName());
            IsOpen = false;
            base.OnSessionClosed(reason);
        }

        protected override void HandleUnknownRequest(BinaryRequestInfo requestInfo)
        {
            Logger.Debug("HandleUnknownRequest ({0})", GetSessionName());
            base.HandleUnknownRequest(requestInfo);
        }

        public void SetUser(User user)
        {
            Logger.Debug("SetUser {0}", GetSessionName());
            User = user;
            Authorized(this, EventArgs.Empty);
        }

        public event EventHandler Authorized = delegate {};

        public User User { get; private set; }

        public bool IsAuthorized { get { return User != null; } }

        public bool IsOpen
        {
            get { return _isOpen && Connected; }
            private set { _isOpen = value; }
        }

        public Task<TResponse> SendRequest<TResponse>(RequestBase request) where TResponse : ResponseBase
        {
            Interlocked.Increment(ref _lastToken);
            request.Token = _lastToken;
            return _requestsHandler.WaitForResponse<TResponse>(request, () => Send(request));
        }

        public Task<TResponse> SendRequest<TResponse, TRequest>() where TResponse : ResponseBase where TRequest : RequestBase
        {
            var instance = Activator.CreateInstance<TRequest>();
            return SendRequest<TResponse>(instance);
        }

        public void Send<T>(T obj) where T : class
        {
            try
            {
                Logger.Trace("SEND ({1}): {0}", obj.GetType().Name, GetSessionName());
                var objBytes = _serializer.Serialize(obj);
                var cmd = CommandNames.Data; 
                if (obj is ResponseBase)
                    cmd = CommandNames.Response;
                if (obj is RequestBase)
                    cmd = CommandNames.Request;

                var commandBytes = _commandParser.ToBytes(cmd, objBytes);
                _performanceMeasurer.HandleOutgoingBytes(commandBytes.LongLength);
                Send(commandBytes, 0, commandBytes.Length);
            }
            catch (Exception exc)
            {
                Logger.Exception(exc, "Tried to send {0} to {1}", obj == null ? "NULL" : obj.GetType().Name, GetSessionName());
                //TODO: log
            }
        }
    }
}
