using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class DirectoratReportConfiguration : IEntityTypeConfiguration<DirectoratReport>
    {
        public void Configure(EntityTypeBuilder<DirectoratReport> builder)
        {
            builder.ToTable("DirectoratReport");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("DRId");


            builder.Property(x => x.ForcastBudgetPercent)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");

            builder.Property(x => x.ForcastFunctionalPercent)
                    .IsRequired()
                    .HasColumnType("decimal(18,6)");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.DirectoratReport)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.DirectoratReport)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SectionType)
                    .WithMany(x => x.DirectoratReportSectionType)
                    .HasForeignKey(x => x.SectionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UnitType)
                    .WithMany(x => x.DirectoratReportUnitType)
                    .HasForeignKey(x => x.UnitTypeId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
