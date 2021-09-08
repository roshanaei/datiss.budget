using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Datiss.Budget.DataLayer.Mappings
{
    public class WasteSalesSplitConfiguration:IEntityTypeConfiguration<WasteSalesSplit>
    {
        public void Configure(EntityTypeBuilder<WasteSalesSplit> builder)
        {
            builder.ToTable("SalesSplitWs_Y");
            
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                    .HasColumnName("SalesSplitWsYID");

            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.UserTypeId).IsRequired();

            builder.Property(x => x.WsPipeDiameterId).IsRequired();

            builder.Property(x => x.NumberSales).IsRequired();

            builder.Property(x => x.UnitSales).IsRequired();

            builder.Property(x => x.AverageCapacity)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");


            builder.HasOne(x => x.FinanceYear)
                .WithMany(x => x.WasteSalesSplits)
                .HasForeignKey(x => x.YearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.WasteSalesSplits)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                .WithMany(x => x.UserTypeWasteSalesSplit)
                .HasForeignKey(x => x.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.WsPipeDiameter)
                .WithMany(x => x.WastepipeDiameterSalesSplit)
                .HasForeignKey(x => x.WsPipeDiameterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
