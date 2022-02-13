using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
   public class CostCurrentInstalationConfiguration :IEntityTypeConfiguration<CostCurrentInstalation>
    {
        public void Configure(EntityTypeBuilder<CostCurrentInstalation> builder)
        {
            builder.ToTable("CostCurrentInstalation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CCInstalationId");

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.CCInstalationTypeId).IsRequired();

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentInstalations)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentInstalations)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CCInstalationType)
                    .WithMany(x => x.CostCurrentInstalations)
                    .HasForeignKey(x => x.CCInstalationTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
