using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostForcastBuyConfiguration : IEntityTypeConfiguration<CostForcastBuy>
    {
        public void Configure(EntityTypeBuilder<CostForcastBuy> builder)
        {
            builder.ToTable("CostForcastBuy")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CFBuyId");

            builder.Property(x => x.BuyDescription)
                    .HasMaxLength(300)
                    .IsUnicode();

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostForcastBuy)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostForcastBuy)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Location)
                    .WithMany(x => x.CostForcastBuyLocation)
                    .HasForeignKey(x => x.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Department)
                    .WithMany(x => x.CostForcastBuyDepartment)
                    .HasForeignKey(x => x.BuyDepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.CostCenter)
                    .WithMany(x => x.CostForcastBuyCostCenter)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Asset)
                    .WithMany(x => x.CostForcastBuyAsset)
                    .HasForeignKey(x => x.AssetTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.AssetDetail)
                    .WithMany(x => x.CostForcastBuyAssetDetail)
                    .HasForeignKey(x => x.AssetDetailTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Measurement)
                    .WithMany(x => x.CostForcastBuyMeasurement)
                    .HasForeignKey(x => x.MeasurementTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Credit)
                    .WithMany(x => x.CostForcastBuyCredit)
                    .HasForeignKey(x => x.CreditTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
