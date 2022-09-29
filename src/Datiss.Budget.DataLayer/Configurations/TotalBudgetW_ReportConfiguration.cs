using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class TotalBudgetWReportConfiguration : IEntityTypeConfiguration<TotalBudgetWReport>
    {
        public void Configure(EntityTypeBuilder<TotalBudgetWReport> builder)
        {
            builder.ToTable("TotalBudgetWReport");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("TBWRId");


            builder.Property(x => x.ForcastBudgetPercent)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");

            builder.Property(x => x.ForcastFunctionalPercent)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.TotalBudgetWReport)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.TotalBudgetWReport)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SectionType)
                    .WithMany(x => x.TotalBudgetWReport)
                    .HasForeignKey(x => x.SectionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UnitType)
                    .WithMany(x => x.TotalBudgetWReportUnitType)
                    .HasForeignKey(x => x.UnitTypeId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
