using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class CostForcastBuyDescriptionConfiguration :IEntityTypeConfiguration<CostForcastBuyDescription>
    {
        public void Configure(EntityTypeBuilder<CostForcastBuyDescription> builder)
        {
            builder.ToTable("CostForcastBuyDescription")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CFBDId");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostForcastBuyDescription)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Asset)
                    .WithMany(x => x.CostForcastBuyDescriptionAssetType)
                    .HasForeignKey(x => x.AssetTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.AssetDetail)
                .WithMany(x => x.CostForcastBuyDescriptionAssetDetailType)
                .HasForeignKey(x => x.AssetDetailTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Measurement)
                .WithMany(x => x.CostForcastBuyDescriptionMeasurementType)
                .HasForeignKey(x => x.MeasurementTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
