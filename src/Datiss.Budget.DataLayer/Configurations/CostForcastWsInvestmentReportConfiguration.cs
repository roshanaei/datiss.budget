using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class CostForcastWsInvestmentReportConfiguration :IEntityTypeConfiguration<CostForcastWsInvestmentReport>
    {
        public void Configure(EntityTypeBuilder<CostForcastWsInvestmentReport> builder)
        {
            builder.ToTable("CostForcastWsInvestmentReport");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CFWsIRId");

            builder.HasOne(x => x.FinanceYear)
                .WithMany(x => x.CostForcastWsInvestmentReport)
                .HasForeignKey(x => x.YearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.CostForcastWsInvestmentReport)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CostCenterType)
                .WithMany(x => x.CostForcastWsInvestmentReportCostCenterType)
                .HasForeignKey(x => x.CostCenterTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SectionType)
                .WithMany(x => x.CostForcastWsInvestmentReportSectionType)
                .HasForeignKey(x => x.SectionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UnitType)
                .WithMany(x => x.CostForcastWsInvestmentReportUnitType)
                .HasForeignKey(x => x.UnitTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
