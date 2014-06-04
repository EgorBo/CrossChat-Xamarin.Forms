using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Crosschat.Server.Domain.Entities;
using Crosschat.Server.Domain.Repositories;
using Crosschat.Server.Domain.Seedwork;
using Crosschat.Server.Infrastructure.Persistence.EF.Mappings;
using Crosschat.Server.Infrastructure.Persistence.EF.Repositories;

namespace Crosschat.Server.Infrastructure.Persistence.EF
{
    public class UnitOfWork : DbContext, IUnitOfWork
    {
        public IUserRepository UsersRepository { get; private set; }
        public IPublicMessageRepository PublicMessageRepository { get; private set; }

        public UnitOfWork()
        {
            base.Configuration.LazyLoadingEnabled = true;
            //Database.Log = s => Debug.WriteLine("SQL:    " + s);

            Users = Set<User>();
            PublicMessages = Set<PublicMessage>();

            UsersRepository = new UserRepository(this);
            PublicMessageRepository = new PublicMessageRepository(this);
        }

        internal IDbSet<User> Users { get; private set; }
        internal IDbSet<PublicMessage> PublicMessages { get; private set; }

        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            //attach and set as unchanged
            base.Entry(item).State = EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : class
        {
            //this operation also attach item in object state manager
            base.Entry(item).State = EntityState.Modified;
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class
        {
            //if it is not attached, attach original and set current values
            base.Entry(original).CurrentValues.SetValues(current);
        }

        public void Commit()
        {
            bool saveFailed = false;
            do
            {
                try
                {
                    base.SaveChanges();
                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries.ToList().ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));
                }
            }
            while (saveFailed);
        }
        
        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            base.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new UserEntityConfiguration());
            modelBuilder.Configurations.Add(new PublicMessageEntityConfiguration());
        }
    }
}