using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class SalesSplitFunctionConfiguration : IEntityTypeConfiguration<SalesSplitFunction>
    {
        public void Configure(EntityTypeBuilder<SalesSplitFunction> builder)
        {
            builder.ToTable("Total_SalesSplit");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("TSalesSplitID");

            builder.HasOne(x => x.FinanceYear)
                .WithMany(x => x.SalesSplitFunctions)
                .HasForeignKey(x => x.YearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.SalesSplitFunctions)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.UserType)
                .WithMany(x => x.SalesSplitFunctions)
                .HasForeignKey(x => x.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
