using System.Data.Entity.ModelConfiguration;
using Crosschat.Server.Domain.Entities;

namespace Crosschat.Server.Infrastructure.Persistence.EF.Mappings
{
    public class UserEntityConfiguration : EntityTypeConfiguration<User>
    {
        public UserEntityConfiguration()
        {
            HasMany(m => m.Friends)
                .WithMany()
                .Map(m =>
                     {
                         m.MapLeftKey("TargetId");
                         m.MapRightKey("TargetUserId");
                         m.ToTable("UserFriends");
                     });
        }
    }
}
