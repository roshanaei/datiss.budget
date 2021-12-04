using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Datiss.Budget.DataLayer.Mappings
{
    public class IncomeCurrentNOperationalConfiguration :IEntityTypeConfiguration<IncomeCurrentNOperational> 
    {
        public void Configure(EntityTypeBuilder<IncomeCurrentNOperational> builder)
        {
            builder.ToTable("IncomeCurrentNOperationals");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("ICNOId");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.IncomeCurrentNOperationals)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.IncomeCurrentNOperationals)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.NOICType)
                    .WithMany(x => x.IncomeCurrentNOperationals)
                    .HasForeignKey(x => x.NOICTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
