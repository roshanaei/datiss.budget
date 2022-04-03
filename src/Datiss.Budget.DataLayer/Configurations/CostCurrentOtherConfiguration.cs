using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentOtherConfiguration : IEntityTypeConfiguration<CostCurrentOther>
    {
        public void Configure(EntityTypeBuilder<CostCurrentOther> builder)
        {
            builder.ToTable("CostCurrentOther").HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("CostCurrentOtherId");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentOther)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentOther)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CostCenter)
                    .WithMany(x => x.CostCenterCostCurrentOther)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CCOtherCosts)
                    .WithMany(x => x.CCOtherCostsCostCurrentOther)
                    .HasForeignKey(x => x.CCOtherCostsTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
