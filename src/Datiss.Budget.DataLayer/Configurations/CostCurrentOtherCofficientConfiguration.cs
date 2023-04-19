using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentOtherCofficientConfiguration : IEntityTypeConfiguration<CostCurrentOtherCofficient>
    {
        public void Configure(EntityTypeBuilder<CostCurrentOtherCofficient> builder)
        {
            builder.ToTable("CostCurrentOtherCofficient").HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("CCOCofficientId");

            builder.Property(x => x.Fee).HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentOtherCofficient)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CostCenter)
                    .WithMany(x => x.CostCurrentOtherCofficientCostCenter)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CCOtherCosts)
                    .WithMany(x => x.CostCurrentOtherCofficientType)
                    .HasForeignKey(x => x.CCOtherCostsTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
