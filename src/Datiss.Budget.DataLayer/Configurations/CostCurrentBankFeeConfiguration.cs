using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentBankFeeConfiguration : IEntityTypeConfiguration<CostCurrentBankFee>
    {
        public void Configure(EntityTypeBuilder<CostCurrentBankFee> builder)
        {
            builder.ToTable("CostCurrentBankFee");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CCBankFeeId");

            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.ActivityType).IsRequired();

            builder.Property(x => x.CostCenterTypeId).IsRequired();

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentBankFee)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentBankFee)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CostCenterType)
                    .WithMany(x => x.CostCurrentBankFee)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
