using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class UserTypeAverageCapacityCurrentConfiguration : IEntityTypeConfiguration<UserTypeAverageCapacityCurrent>
    {
        public void Configure(EntityTypeBuilder<UserTypeAverageCapacityCurrent> builder)
        {
            builder.ToTable("UserTypeAverageCapacityCurrent");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("UTACCId");

            builder.Property(x => x.AverageCapacityWIncome).HasColumnType("decimal(18,6)");

            builder.Property(x => x.AverageCapacityWsIncome).HasColumnType("decimal(18,6)");

            builder.Property(x => x.SummerIndex).HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.UserTypeAverageCapacityCurrents)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.UserTypeAverageCapacityCurrents)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.UserTypeAverageCapacityCurrents)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}