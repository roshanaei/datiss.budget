using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class FeeCityConfiguration :IEntityTypeConfiguration<FeeCity>
    {
        public void Configure(EntityTypeBuilder<FeeCity> builder)
        {
            builder.ToTable("FeeCity");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("FeeCityId");

            builder.Property(x => x.DomesticPrice)
                    .HasColumnType("decimal(18,6)");

            builder.Property(x => x.NDomesticPrice)
                    .HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.FeeCity)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.FeeCity)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
