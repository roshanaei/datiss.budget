using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentNOConfiguration : IEntityTypeConfiguration<CostCurrentNO>
    {
        public void Configure(EntityTypeBuilder<CostCurrentNO> builder)
        {
            builder.ToTable("CostCurrentNO").HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CostCurrentNOId");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentNO)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentNO)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CostCurrentNoType)
                    .WithMany(x => x.CostCurrentNO)
                    .HasForeignKey(x => x.CostCurrentNoTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
