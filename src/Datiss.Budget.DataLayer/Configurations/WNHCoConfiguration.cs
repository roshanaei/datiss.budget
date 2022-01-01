using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Datiss.Budget.DataLayer.Mappings
{
    public class WNHCoConfiguration : IEntityTypeConfiguration<WNHCo>
    {
        public void Configure(EntityTypeBuilder<WNHCo> builder)
        {
            builder.ToTable("WNHCo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("WNHCoId");

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.WNHCo)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.WNHCo)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
