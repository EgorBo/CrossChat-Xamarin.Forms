using System.Data.Entity;

namespace Crosschat.Server.Infrastructure.Persistence.EF
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<UnitOfWork>
    {
        protected override void Seed(UnitOfWork context)
        {
            base.Seed(context);
        }
    }
}
