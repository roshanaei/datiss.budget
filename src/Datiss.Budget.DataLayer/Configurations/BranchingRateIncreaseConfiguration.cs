using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class BranchingRateIncreaseConfiguration : IEntityTypeConfiguration<BranchingRateIncrease>
    {
        public void Configure(EntityTypeBuilder<BranchingRateIncrease> builder)
        {
            builder.ToTable("BranchingRateIncrease");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("BranchingRateIncreaseId");


            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.BranchingRateIncrease)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.BranchingRateIncrease)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.BranchingRateIncrease)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
