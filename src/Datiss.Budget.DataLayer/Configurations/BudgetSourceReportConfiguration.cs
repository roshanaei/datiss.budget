using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class BudgetSourceReportConfiguration : IEntityTypeConfiguration<BudgetSourceReport>
    {
        public void Configure(EntityTypeBuilder<BudgetSourceReport> builder)
        {
            builder.ToTable("BudgetSourceReports");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("BudgetSourceReportId");

            builder.Property(x => x.ReceiptPercent)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");

            builder.Property(x => x.ForcastBudgetPercent)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");

            builder.Property(x => x.ForcastFunctionalPercent)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.BudgetSourceReports)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.BudgetSourceReports)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SectionType)
                    .WithMany(x => x.BudgetSourceReports)
                    .HasForeignKey(x => x.SectionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
