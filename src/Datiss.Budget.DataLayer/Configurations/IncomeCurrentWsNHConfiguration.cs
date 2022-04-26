using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class IncomeCurrentWsNHConfiguration : IEntityTypeConfiguration<IncomeCurrentWsNH>
    {
        public void Configure(EntityTypeBuilder<IncomeCurrentWsNH> builder)
        {
            builder.ToTable("IncomeCurrentWsNH").HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("IncomeCurrentWsNHId");

            builder.Property(x => x.AvgConsumeUser).HasColumnType("decimal(18,6)");

            builder.Property(x => x.Capacity).HasColumnType("decimal(18,6)");

            builder.Property(x => x.ExcessConsumption).HasColumnType("decimal(18,6)");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.IncomeCurrentWsNH)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.IncomeCurrentWsNH)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                .WithMany(x => x.IncomeCurrentWsNH)
                .HasForeignKey(x => x.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
