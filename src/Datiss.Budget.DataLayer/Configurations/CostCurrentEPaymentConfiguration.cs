using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Datiss.Budget.DataLayer.Mappings
{
   public class CostCurrentEPaymentConfiguration :IEntityTypeConfiguration<CostCurrentEPayment>
    {
        public void Configure(EntityTypeBuilder<CostCurrentEPayment> builder)
        {
            builder.ToTable("CostCurrentEPayment");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CCEPaymentId");
            
            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentEPayment)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentEPayment)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
