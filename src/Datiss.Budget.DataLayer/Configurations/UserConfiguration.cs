using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AppUsers");

            builder.Property(x => x.FirstName)
                .HasMaxLength(450)
                .IsUnicode()
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(450)
                .IsUnicode()
                .IsRequired();

            builder.Property(x => x.Status)
                .HasDefaultValue(EntityStatus.Enabled);

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Position)
                .WithMany(x => x.UserPositions)
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}