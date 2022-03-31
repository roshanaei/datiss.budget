using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Datiss.Budget.Enum;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class CostCurrentPMDepConfiguration : IEntityTypeConfiguration<CostCurrentPMDep>
    {
        public void Configure(EntityTypeBuilder<CostCurrentPMDep> builder)
        {
            builder.ToTable("CostCurrentPMDeps");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("CostCurrentPMDepId");

            builder.Property(x => x.RFinancePMCost_D)
                .IsRequired()
                .HasColumnType("decimal(18,6)");

            builder.Property(x => x.RFinanceDepCost_D)
                .IsRequired()
                .HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.FinanceYear)
                .WithMany(x => x.CostCurrentPMDeps)
                .HasForeignKey(x => x.YearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.CostCurrentPMDeps)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CCPMDepType)
                .WithMany(x => x.CostCurrentPMDeps)
                .HasForeignKey(x => x.CCPMDepTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CostCenterType)
                .WithMany(x => x.CostCenterCostCurrentPMDeps)
                .HasForeignKey(x => x.CostCenterTypeId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
