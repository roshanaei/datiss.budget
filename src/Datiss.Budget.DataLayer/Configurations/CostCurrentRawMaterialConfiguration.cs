using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentRawMaterialConfiguration : IEntityTypeConfiguration<CostCurrentRawMaterial>
    {
        public void Configure(EntityTypeBuilder<CostCurrentRawMaterial> builder)
        {
            builder.ToTable("CostCurrentRawMaterial")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                 .HasColumnName("CCRMId");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentRawMaterial)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                   .WithMany(x => x.CostCurrentRawMaterial)
                   .HasForeignKey(x => x.OrganizationId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.RawMaterial)
                    .WithMany(x => x.CostCurrentRawMaterial)
                    .HasForeignKey(x => x.RawMaterialTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
