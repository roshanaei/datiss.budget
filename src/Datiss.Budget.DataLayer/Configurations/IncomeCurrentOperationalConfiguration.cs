
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class IncomeCurrentOperationalConfiguration :IEntityTypeConfiguration<IncomeCurrentOperational>
    {
        public void Configure(EntityTypeBuilder<IncomeCurrentOperational> builder)
        {
            builder.ToTable("IncomeCurrentOperationals").HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("ICOId");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.IncomeCurrentOperationals)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.IncomeCurrentOperationals)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ICOType)
                    .WithMany(x => x.IncomeCurrentOperationals)
                    .HasForeignKey(x => x.ICOTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
        }
    }
}
