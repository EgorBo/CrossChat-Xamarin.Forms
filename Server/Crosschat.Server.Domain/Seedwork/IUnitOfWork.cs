using System;
using Crosschat.Server.Domain.Repositories;

namespace Crosschat.Server.Domain.Seedwork
{
    /// <summary>
    /// Contract for "UnitOfWork pattern"
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UsersRepository { get; }

        IPublicMessageRepository PublicMessageRepository { get; }


        void Attach<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown
        ///</remarks>
        void Commit();
        
        /// <summary>
        /// Rollback tracked changes. See references of UnitOfWork pattern
        /// </summary>
        void RollbackChanges();
    }
}
