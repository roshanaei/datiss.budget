using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{

    public class WasteInstallFeeConfiguration : IEntityTypeConfiguration<WasteInstallFee>
    {
        public void Configure(EntityTypeBuilder<WasteInstallFee> builder)
        {
            builder.ToTable("WasteInstallFees");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("WasteInstallFeeId");

            builder.Property(x => x.YearId).IsRequired();

            builder.Property(x => x.OrganizationId).IsRequired();

            builder.Property(x => x.DWasteTypeId).IsRequired();

            builder.Property(x => x.WsInstallFee).IsRequired();

            builder.HasOne(x => x.FinanceYear)
                .WithMany(x => x.WasteInstallFees)
                .HasForeignKey(x => x.YearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.WasteInstallFees)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DWasteType)
                .WithMany(x => x.WasteInstallFees)
                .HasForeignKey(x => x.DWasteTypeId)
                .OnDelete(DeleteBehavior.Restrict);

          
        }
    }

}