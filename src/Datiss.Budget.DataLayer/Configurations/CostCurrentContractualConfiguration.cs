using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentContractualConfiguration : IEntityTypeConfiguration<CostCurrentContractual>
    {
        public void Configure(EntityTypeBuilder<CostCurrentContractual> builder)
        {
            builder.ToTable("CostCurrentContractual");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CCContractualId");

            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.CostCenterTypeId).IsRequired();

            builder.Property(x => x.ExtensionId).IsRequired();

            builder.Property(x => x.ContractDescription)
                    .HasMaxLength(400)
                    .IsUnicode()
                    .IsRequired();



            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentContractual)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentContractual)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CostCenterType)
                    .WithMany(x => x.CostCurrentContractual)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
