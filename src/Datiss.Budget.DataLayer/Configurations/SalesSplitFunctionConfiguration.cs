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

            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.UserTypeId).IsRequired();





        }
    }
}
