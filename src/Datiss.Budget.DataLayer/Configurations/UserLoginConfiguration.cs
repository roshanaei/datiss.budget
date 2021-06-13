using Datiss.Budget.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.HasOne(userLogin => userLogin.User)
                   .WithMany(user => user.Logins)
                   .HasForeignKey(userLogin => userLogin.UserId);

            builder.ToTable("AppUserLogins");
        }
    }
}