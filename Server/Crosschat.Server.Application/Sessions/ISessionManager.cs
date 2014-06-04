using System;
using System.Collections.Generic;

namespace Crosschat.Server.Application.Sessions
{
    public interface ISessionManager
    {
        /// <summary>
        /// Get all currently active sessions
        /// </summary>
        Dictionary<int, ISession> GetActiveSessions();

        /// <summary>
        /// Send data to all sessions in the chat
        /// </summary>
        void SendToEachChatSessions<T>(T data) where T : class;

        /// <summary>
        /// Send data to all sessions in the chat
        /// </summary>
        void SendToEachChatSessionsExcept<T>(T data, int exceptionPlayerId) where T : class;

        /// <summary>
        /// Try to find session by associated User id
        /// </summary>
        /// <returns>null if session wasn't found</returns>
        ISession FindSessionByPlayerId(int playerId);
        
        /// <summary>
        /// Forcebly close session with given User id
        /// </summary>
        void CloseSessionByPlayerId(int id);

        /// <summary>
        /// Authenticated user started session
        /// </summary>
        event EventHandler<SessionEventArgs> AuthenticatedUserConnected;

        /// <summary>
        /// Authenticated user ended session
        /// </summary>
        event EventHandler<SessionEventArgs> AuthenticatedUserDisconnected;
    }
}