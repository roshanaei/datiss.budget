using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentConsumableConfiguration : IEntityTypeConfiguration<CostCurrentConsumable>
    {
        public void Configure(EntityTypeBuilder<CostCurrentConsumable> builder)
        {
            builder.ToTable("CostCurrentConsumable");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CCConsumableId");

            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.ActivityType).IsRequired();

            builder.Property(x => x.ConsumableTypeId).IsRequired();

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentConsumable)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentConsumable)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ConsumableType)
                    .WithMany(x => x.CostCurrentConsumable)
                    .HasForeignKey(x => x.ConsumableTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
