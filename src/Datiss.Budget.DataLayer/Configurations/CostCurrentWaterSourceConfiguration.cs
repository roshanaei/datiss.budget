using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentWaterSourceConfiguration :IEntityTypeConfiguration<CostCurrentWaterSource>
    {
        public void Configure(EntityTypeBuilder<CostCurrentWaterSource> builder)
        {
            builder.ToTable("CostCurrentWaterSource").HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CCWaterSourceId");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentWaterSource)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentWaterSource)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.WaterSourceType)
                    .WithMany(x => x.CostCurrentWaterSource)
                    .HasForeignKey(x => x.WaterSourceTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
