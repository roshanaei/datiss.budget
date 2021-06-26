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

            builder.Property(x => x.IpAddress)
                .HasMaxLength(50)
                .IsUnicode();

            builder.Property(x => x.HostName)
                .HasMaxLength(200)
                .IsUnicode();

            builder.Property(x => x.UserAgent)
                .HasMaxLength(255)
                .IsUnicode();

            builder.ToTable("AppUserLogins");
        }
    }
}