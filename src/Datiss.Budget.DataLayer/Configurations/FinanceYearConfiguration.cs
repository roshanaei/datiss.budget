using Datiss.Budget.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{

    public class FinanceYearConfiguration : IEntityTypeConfiguration<FinanceYear>
    {
        public void Configure(EntityTypeBuilder<FinanceYear> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("FinanceYearId");

            builder.Property(x => x.Title).HasMaxLength(400).IsRequired();
            builder.Property(x => x.Year).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();
            builder.ToTable("FinanceYears");
        }
    }

}