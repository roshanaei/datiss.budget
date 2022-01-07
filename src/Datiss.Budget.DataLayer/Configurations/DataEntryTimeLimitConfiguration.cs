using Datiss.Budget.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class DataEntryTimeLimitConfiguration : IEntityTypeConfiguration<DataEntryTimeLimit>
    {

        public void Configure(EntityTypeBuilder<DataEntryTimeLimit> builder) 
        {
            builder.ToTable("DataEntryTimeLimits");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description).HasMaxLength(1000).IsUnicode();

            builder.HasOne(x=> x.Organization)
                .WithMany(x=> x.DataEntryTimeLimits)
                .HasForeignKey(x=> x.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Year)
                .WithMany(x => x.DataEntryTimeLimits)
                .HasForeignKey(x => x.YearId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(x => x.DataEntryTimeLimits)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.DataEntryTimeLimits)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
