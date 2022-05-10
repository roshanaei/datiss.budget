using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{

    public class CostCurrentWaterSourcePriceConfiguration : IEntityTypeConfiguration<CostCurrentWaterSourcePrice>
    {
        public void Configure(EntityTypeBuilder<CostCurrentWaterSourcePrice> builder)
        {

            builder.ToTable("CostCurrentWaterSourcePrices");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CostCurrentWaterSourcePriceId");

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.WaterSourceTypeId).IsRequired();

            builder.Property(x => x.Price).IsRequired();

            builder.HasOne(x => x.FinanceYear)
                .WithMany(x => x.CostCurrentWaterSourcePrices)
                .HasForeignKey(x => x.YearId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(x => x.Organization)
                .WithMany(x => x.CostCurrentWaterSourcePrices)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(x => x.WaterSourceType)
                .WithMany(x => x.CostCurrentWaterSourcePrices)
                .HasForeignKey(x => x.WaterSourceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }

}