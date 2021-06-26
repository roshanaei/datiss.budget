using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{

    public class WaterInstallFeeConfiguration : IEntityTypeConfiguration<WaterInstallFee>
    {
        public void Configure(EntityTypeBuilder<WaterInstallFee> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("WaterInstallFeeId");
            builder.HasOne(x => x.FinanceYear).WithMany(x => x.WaterInstallFees).HasForeignKey(x => x.YearId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Organization).WithMany(x => x.WaterInstallFees).HasForeignKey(x => x.OrganizationId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.DWaterType).WithMany(x => x.WaterInstallFees).HasForeignKey(x => x.DWaterTypeId).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("WaterInstallFees");
        }
    }

}