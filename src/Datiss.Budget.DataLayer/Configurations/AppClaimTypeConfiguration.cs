using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class AppClaimTypeConfiguration : IEntityTypeConfiguration<AppClaimType>
    {
        public void Configure(EntityTypeBuilder<AppClaimType> builder) {
            builder.ToTable("AppClaimTypes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(255).IsUnicode().IsRequired();
            builder.Property(x => x.Status).HasDefaultValue(EntityStatus.Enabled);
            builder.Property(x => x.Title).HasMaxLength(255).IsUnicode();
        }

    }
}
