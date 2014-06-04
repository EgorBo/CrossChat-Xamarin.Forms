using System;
using System.Threading.Tasks;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Server.Domain.Entities;

namespace Crosschat.Server.Application.Sessions
{
    public interface ISession
    {
        /// <summary>
        /// Set up User obj for the session
        /// </summary>
        void SetUser(User user);

        /// <summary>
        /// Associated User
        /// </summary>
        User User { get; }

        /// <summary>
        /// Does it associated with User (and he\she is registered)
        /// </summary>
        bool IsAuthorized { get; }

        /// <summary>
        /// </summary>
        event EventHandler Authorized;

        /// <summary>
        /// Session is healty or not
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Send data to remote peer
        /// </summary>
        void Send<T>(T obj) where T : class;
        
        /// <summary>
        /// Sena a request and wait for a response
        /// </summary>
        Task<TResponse> SendRequest<TResponse>(RequestBase request) where TResponse : ResponseBase;

        /// <summary>
        /// Sena a request and wait for a response
        /// </summary>
        Task<TResponse> SendRequest<TResponse, TRequest>() where TResponse : ResponseBase where TRequest : RequestBase;
    }
}
