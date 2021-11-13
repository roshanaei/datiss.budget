using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class IncomeForcastWsConfiguration : IEntityTypeConfiguration<IncomeForcastWs>
    {
        public void Configure(EntityTypeBuilder<IncomeForcastWs> builder)
        {
            builder.ToTable("IncomeForcastWs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("IncomeForcastWsId");

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.IncomeForcastWs)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.IncomeForcastWs)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.IncomeForcastWs)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
