using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class UserTypeAverageCapacityConfiguration : IEntityTypeConfiguration<UserTypeAverageCapacity>
    {
        public void Configure(EntityTypeBuilder<UserTypeAverageCapacity> builder)
        {
            builder.ToTable("UserTypeAverageCapacity");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("UTACID");

            builder.Property(x => x.AverageCapacityW).HasColumnType("decimal(18,6)");

            builder.Property(x => x.AverageCapacityWs).HasColumnType("decimal(18,6)");

            builder.Property(x => x.AverageCapacityWIncome).HasColumnType("decimal(18,6)");

            builder.Property(x => x.AverageCapacityWsIncome).HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.UserTypeAverageCapacities)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.UserTypeAverageCapacities)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.UserTypeAverageCapacities)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}