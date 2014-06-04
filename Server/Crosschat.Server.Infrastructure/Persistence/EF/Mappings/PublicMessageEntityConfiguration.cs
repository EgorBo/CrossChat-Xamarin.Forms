using System.Data.Entity.ModelConfiguration;
using Crosschat.Server.Domain.Entities;

namespace Crosschat.Server.Infrastructure.Persistence.EF.Mappings
{
    public class PublicMessageEntityConfiguration : EntityTypeConfiguration<PublicMessage>
    {
        public PublicMessageEntityConfiguration()
        {
        }
    }
}