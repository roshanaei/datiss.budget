using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class CostCurrentReportConfiguration : IEntityTypeConfiguration<CostCurrentReport>
    {
        public void Configure(EntityTypeBuilder<CostCurrentReport> builder)
        {
            builder.ToTable("CostCurrentReports");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CostCurrentReportId");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentReports)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentReports)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SectionType)
                    .WithMany(x => x.CostCurrentReportSection)
                    .HasForeignKey(x => x.SectionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UnitType)
                    .WithMany(x => x.CostCurrentReportUnit)
                    .HasForeignKey(x => x.UnitTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UnitDetailType)
                    .WithMany(x => x.CostCurrentReportUnitDetail)
                    .HasForeignKey(x => x.UnitDetailTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CostCenterType)
                    .WithMany(x => x.CostCurrentReportCostCenter)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
