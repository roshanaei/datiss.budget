using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Datiss.Budget.DataLayer.Mappings
{
    public class NHCoConfiguration : IEntityTypeConfiguration<NHCo>
    {
        public void Configure(EntityTypeBuilder<NHCo> builder)
        {
            builder.ToTable("NHCo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("NHCoId");

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.NHCo)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.NHCo)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
