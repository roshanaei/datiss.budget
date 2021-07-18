using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class SalesSplitW_YConfiguration :IEntityTypeConfiguration<SalesSplitW_Y>
    {
        public void Configure(EntityTypeBuilder<SalesSplitW_Y> builder) 
        {
            builder.ToTable("SalesSplitW_Ys");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("SalesSplitWYID");

            builder.HasOne(x => x.FinanceYear).WithMany(x => x.SalesSplitW_Ys).HasForeignKey(x => x.YearId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization).WithMany(x => x.SalesSplitW_Ys).HasForeignKey(x => x.OrganizationId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType).WithMany(x => x.SalesSplitW_Ys).HasForeignKey(x => x.UserTypeId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.WPipeDiameter).WithMany(x => x.SalesSplitW_Ys).HasForeignKey(x => x.WPipeDiameterId).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
