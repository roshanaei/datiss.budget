using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class CostForcastFinanceConfiguration : IEntityTypeConfiguration<CostForcastFinance>
    {
        public void Configure(EntityTypeBuilder<CostForcastFinance> builder)
        {
            builder.ToTable("CostForcastFinance")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CostForcastFinanceId");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostForcastFinance)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostForcastFinance)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CostCenter)
                    .WithMany(x => x.CostForcastFinanceCostCenter)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FinanceSubject)
                    .WithMany(x => x.CostForcastFinanceFinanceSubject)
                    .HasForeignKey(x => x.FinanceSubjectTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
