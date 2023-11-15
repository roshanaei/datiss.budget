using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentSharingSetadConfiguration :IEntityTypeConfiguration<CostCurrentSharingSetad>
    {
        public void Configure(EntityTypeBuilder<CostCurrentSharingSetad> builder)
        {
            builder.ToTable("CostCurrentSharingSetad").HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("CostCurrentSharingSetadId");


            builder.Property(x => x.IncomeCurrentWSharingCoff).HasColumnType("decimal(18,6)");

            builder.Property(x => x.IncomeCurrentWsSharingCoff).HasColumnType("decimal(18,6)");

            builder.Property(x => x.IncomeForcastsharing).HasColumnType("decimal(18,6)");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentSharingSetad)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentSharingSetad)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
