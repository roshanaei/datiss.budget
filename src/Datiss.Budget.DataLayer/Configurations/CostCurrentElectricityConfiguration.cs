using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentElectricityConfiguration :IEntityTypeConfiguration<CostCurrentElectricity>
    {
        public void Configure(EntityTypeBuilder<CostCurrentElectricity> builder)
        {
            builder.ToTable("CostCurrentElectricity");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("CCElectricityId");

            builder.Property(x => x.YearId)
                    .IsRequired();

            builder.Property(x => x.OrganizationId)
                    .IsRequired();

            builder.Property(x => x.ActivityType)
                    .IsRequired();

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentElectricity)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentElectricity)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
