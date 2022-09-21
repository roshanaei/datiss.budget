using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class CostForcastConsumptionReportConfiguration :IEntityTypeConfiguration<CostForcastConsumptionReport>
    {
        public void Configure(EntityTypeBuilder<CostForcastConsumptionReport> builder)
        {
            builder.ToTable("CostForcastConsumptionReport");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CostForcastConsumptionReportId");

            builder.Property(x => x.ReceiptPercent)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");

            builder.Property(x => x.ForcastBudgetPercent)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");

            builder.Property(x => x.ForcastFunctionalPercent)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostForcastConsumptionReport)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostForcastConsumptionReport)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SectionType)
                    .WithMany(x => x.CostForcastConsumptionReport)
                    .HasForeignKey(x => x.SectionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
