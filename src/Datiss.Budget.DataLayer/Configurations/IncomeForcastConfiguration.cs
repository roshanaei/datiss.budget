using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
   public class IncomeForcastConfiguration : IEntityTypeConfiguration<IncomeForcast>
    {
        public void Configure(EntityTypeBuilder<IncomeForcast> builder)
        {
            builder.ToTable("IncomeForcast");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("IncomeForcastId");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.IncomeForcasts)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.IncomeForcasts)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                .WithMany(x => x.IncomeForcasts)
                .HasForeignKey(x => x.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
