using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class SalesSplitW_YConfiguration :IEntityTypeConfiguration<SalesSplitWater>
    {
        public void Configure(EntityTypeBuilder<SalesSplitWater> builder) 
        {
            builder.ToTable("SalesSplitW_Y");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("SalesSplitWYID");

            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.UserTypeId).IsRequired();

            builder.Property(x => x.WPipeDiameterId).IsRequired();


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.SalesSplitW_Ys)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.SalesSplitW_Ys)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.UserTypeSalesSplitW)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.WPipeDiameter)
                    .WithMany(x => x.WaterPipeDiameterSalesSplitW)
                    .HasForeignKey(x => x.WPipeDiameterId)
                    .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
