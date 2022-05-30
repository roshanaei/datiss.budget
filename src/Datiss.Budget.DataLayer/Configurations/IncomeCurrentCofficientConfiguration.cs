using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class IncomeCurrentCofficientConfiguration : IEntityTypeConfiguration<IncomeCurrentCofficient>
    {
        public void Configure(EntityTypeBuilder<IncomeCurrentCofficient> builder)
        {
            builder.ToTable("IncomeCurrentCofficient").HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("IncomeCurrentCofficientId");

            builder.Property(x => x.Fee).HasColumnType("decimal(18,6)");

            builder.Property(x => x.FeeWs).HasColumnType("decimal(18,6)");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.IncomeCurrentCofficients)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.IncomeCurrentCofficients)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.UserTypeIncomeCurrentCofficients)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsageLayer)
                    .WithMany(x => x.UsageLayerIncomeCurrentCofficients)
                    .HasForeignKey(x => x.UsageLayerId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
