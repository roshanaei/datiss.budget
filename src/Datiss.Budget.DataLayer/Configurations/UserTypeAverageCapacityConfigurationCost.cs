using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class UserTypeAverageCapacityConfigurationCost : IEntityTypeConfiguration<UserTypeAverageCapacityCost>
    {
        public void Configure(EntityTypeBuilder<UserTypeAverageCapacityCost> builder)
        {
            builder.ToTable("UserTypeAverageCapacityCost");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("UTACCID");

            builder.Property(x => x.AverageCapacityWIncome).HasColumnType("decimal(18,6)");

            builder.Property(x => x.AverageCapacityWsIncome).HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.UserTypeAverageCapacityCosts)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.UserTypeAverageCapacityCosts)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.UserTypeAverageCapacityCosts)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}