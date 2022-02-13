using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
   public class IncomeCurrentInstalationConfiguration :IEntityTypeConfiguration<IncomeCurrentInstalation>
    {
        public void Configure(EntityTypeBuilder<IncomeCurrentInstalation> builder)
        {
            builder.ToTable("IncomeCurrentInstalation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("ICInstalationId");

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.ICInstalationTypeId).IsRequired();

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.IncomeCurrentInstalations)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.IncomeCurrentInstalations)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ICInstalationType)
                    .WithMany(x => x.IncomeCurrentInstalations)
                    .HasForeignKey(x => x.ICInstalationTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
