using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Application.Sessions;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Crosschat.Server.Infrastructure.Transport
{
    public class CrosschatSocketServer : AppServer<CrosschatSession, BinaryRequestInfo>, ISessionManager
    {
        private readonly TransportPerformanceMeasurer _performanceMeasurer = ServiceLocator.Resolve<TransportPerformanceMeasurer>();

        public CrosschatSocketServer()
            : base(new DefaultReceiveFilterFactory<CrosschatReceiveFilter, BinaryRequestInfo>())
        {
        }

        protected override void ExecuteCommand(CrosschatSession session, BinaryRequestInfo requestInfo)
        {
            _performanceMeasurer.HandleIncomingBytes(requestInfo.Body.LongLength);
            base.ExecuteCommand(session, requestInfo);
        }

        public Dictionary<int, ISession> GetActiveSessions()
        {
            var sessions = base.GetSessions(s => s.IsAuthorized && s.IsOpen).ToDictionarySafe(i => i.User.Id, i => i as ISession);
            return sessions;
        }

        public void SendToEachChatSessions<T>(T data) where T : class
        {
            var allSessions = GetActiveSessions();
            Task.Run(() => Parallel.ForEach(allSessions, session => session.Value.Send(data)));
        }

        public void SendToEachChatSessionsExcept<T>(T data, int exceptionPlayerId) where T : class
        {
            var allSessions = GetActiveSessions();
            
            if (allSessions.ContainsKey(exceptionPlayerId))
                allSessions.Remove(exceptionPlayerId);

            Task.Run(() => Parallel.ForEach(allSessions, session => session.Value.Send(data)));
        }

        public ISession FindSessionByUserId(int playerId)
        {
            return base.GetSessions(s => s.IsAuthorized && s.User.Id == playerId && s.IsOpen).Last();
        }

        public void CloseSessionByUserId(int playerId)
        {
            var sessions = base.GetSessions(s => s.IsAuthorized && s.User.Id == playerId && s.IsOpen).ToList();
            foreach (var session in sessions)
            {
                try
                {
                    session.Close();
                }
                catch { }
            }
        }

        protected override void OnNewSessionConnected(CrosschatSession session)
        {
            _performanceMeasurer.HandleNewConnection();
            session.Authorized += session_OnAuthorized;
            base.OnNewSessionConnected(session);
        }

        protected override void OnSessionClosed(CrosschatSession session, CloseReason reason)
        {
            session.Authorized -= session_OnAuthorized;
            base.OnSessionClosed(session, reason);
            if (session.IsAuthorized)
            {
                AuthenticatedUserDisconnected(this, new SessionEventArgs(session));
            }
        }

        private void session_OnAuthorized(object sender, EventArgs e)
        {
            AuthenticatedUserConnected(this, new SessionEventArgs(sender as ISession));
        }

        public event EventHandler<SessionEventArgs> AuthenticatedUserConnected = delegate { };

        public event EventHandler<SessionEventArgs> AuthenticatedUserDisconnected = delegate { };
    }
}
