using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
  public class IncomeCurrentWHConfiguration : IEntityTypeConfiguration<IncomeCurrentWH>
    {
        public void Configure(EntityTypeBuilder<IncomeCurrentWH> builder)
        {
            builder.ToTable("IncomeCurrentWH").HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("IncomeCurrentWHId");

            builder.Property(x => x.AvgConsumeUser).HasColumnType("decimal(18,6)");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.IncomeCurrentWH)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.IncomeCurrentWH)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                .WithMany(x => x.IncomeCurrentWH)
                .HasForeignKey(x => x.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsageLayer)
                    .WithMany(x => x.UsageLayerIncomeCurrentWH)
                    .HasForeignKey(x => x.UsageLayerId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
