using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class SalesSplitTotalConfiguration : IEntityTypeConfiguration<SalesSplitTotal>
    {
        public void Configure(EntityTypeBuilder<SalesSplitTotal> builder)
        {
            builder.ToTable("SalesSplitTotal");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("SalesSplitTotalId");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.SalesSplitTotal)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.SalesSplitTotal)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.SalesSplitTotal)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
