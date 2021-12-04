using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class IncomeForcastOtherConfiguration : IEntityTypeConfiguration<IncomeForcastOther>
    {
        public void Configure(EntityTypeBuilder<IncomeForcastOther> builder)
        {
            builder.ToTable("IncomeForcastOthers").HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("IFOId");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.IncomeForcastOthers)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.IncomeForcastOthers)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.OIFType)
                    .WithMany(x => x.IncomeForcastOthers)
                    .HasForeignKey(x => x.OIFTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
