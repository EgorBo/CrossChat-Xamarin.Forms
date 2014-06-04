using System.Collections.Generic;
using Crosschat.Server.Domain.Entities;
using Crosschat.Server.Domain.Seedwork.Specifications;

namespace Crosschat.Server.Domain.Repositories
{
    public interface IPublicMessageRepository : IRepository
    {
        /// <summary>
        /// Takes specified amout of last inserted messages
        /// </summary>
        IEnumerable<PublicMessage> TakeLast(int count);

        /// <summary>
        /// Counts all messages matching specified specification
        /// </summary>
        int Count(Specification<PublicMessage> spec);

        /// <summary>
        /// Adds message to repo
        /// </summary>
        void Add(PublicMessage msg);
    }
}
