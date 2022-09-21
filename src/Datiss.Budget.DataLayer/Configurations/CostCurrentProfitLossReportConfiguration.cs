using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class CostCurrentProfitLossReportConfiguration : IEntityTypeConfiguration<CostCurrentProfitLossReport>
    {
        public void Configure(EntityTypeBuilder<CostCurrentProfitLossReport> builder)
        {
            builder.ToTable("CostCurrentProfitLossReport");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CCPRId");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentProfitLossReport)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentProfitLossReport)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SectionType)
                    .WithMany(x => x.CostCurrentProfitLossReport)
                    .HasForeignKey(x => x.SectionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
