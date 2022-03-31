using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentFinancingConfiguration :IEntityTypeConfiguration<CostCurrentFinancing>
    {
        public void Configure(EntityTypeBuilder<CostCurrentFinancing> builder)
        {
            builder.ToTable("CostCurrentFinancing");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CCFinancingId");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentFinancing)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentFinancing)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FinancialCostType)
                    .WithMany(x => x.CostCurrentFinancing)
                    .HasForeignKey(x => x.FinancialCostTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
