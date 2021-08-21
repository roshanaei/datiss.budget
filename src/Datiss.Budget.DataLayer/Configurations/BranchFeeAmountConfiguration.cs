using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
   public  class BranchFeeAmountConfiguration :IEntityTypeConfiguration<BranchFeeAmount>
    {
        public void Configure(EntityTypeBuilder<BranchFeeAmount> builder)
        {
            builder.ToTable("WaterWasteBranchingAmount");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");

            builder.Property(x => x.UrbanAdjustmentFactor).HasColumnType("decimal(18,6)");

            builder.Property(x => x.WasteRateInWater).HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.FinanceYear).WithMany(x => x.BranchFeeAmounts)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization).WithMany(x => x.BranchFeeAmounts)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
