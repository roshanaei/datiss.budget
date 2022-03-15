using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CurrentIncomeCofficientConfiguration : IEntityTypeConfiguration<CurrentIncomeCofficient>
    {
        public void Configure(EntityTypeBuilder<CurrentIncomeCofficient> builder)
        {
            builder.ToTable("CurrentIncomeCofficient").HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CurrentIncomeCofficientId");

            builder.Property(x => x.Fee).HasColumnType("decimal(18,6)");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CurrentIncomeCofficients)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CurrentIncomeCofficients)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.UserTypeCurrentIncomeCofficients)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsageLayer)
                    .WithMany(x => x.UsageLayerCurrentIncomeCofficients)
                    .HasForeignKey(x => x.UsageLayerId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
