using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class CostForcastWInvestmentReportConfiguration :IEntityTypeConfiguration<CostForcastWInvestmentReport>
    {
        public void Configure(EntityTypeBuilder<CostForcastWInvestmentReport> builder)
        {
            builder.ToTable("CostForcastWInvestmentReport");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CFWIRId");

            builder.HasOne(x => x.FinanceYear)
                .WithMany(x => x.CostForcastWInvestmentReport)
                .HasForeignKey(x => x.YearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.CostForcastWInvestmentReport)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CostCenterType)
                .WithMany(x => x.CostForcastWInvestmentReportCostCenterType)
                .HasForeignKey(x => x.CostCenterTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SectionType)
                .WithMany(x => x.CostForcastWInvestmentReportSectionType)
                .HasForeignKey(x => x.SectionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UnitType)
                .WithMany(x => x.CostForcastWInvestmentReportUnitType)
                .HasForeignKey(x => x.UnitTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
