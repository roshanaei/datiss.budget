using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class AverageContractedCapacityNHUsesConfiguration : IEntityTypeConfiguration<AverageContractedCapacityNHUses>
    {
        public void Configure(EntityTypeBuilder<AverageContractedCapacityNHUses> builder)
        {
            builder.ToTable("UserTypeAverageCapacity_Y");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("UTACY_ID");

            builder.Property(x => x.AverageCapacity).HasColumnType("decimal(18,6)");

            builder.Property(x => x.AverageCapacityIncome).HasColumnType("decimal(18,6)");

            builder.Property(x => x.AverageCapacityWs).HasColumnType("decimal(18,6)");

            builder.Property(x => x.AverageCapacityWsIncome).HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.AverageContractedCapacityNHUses)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.AverageContractedCapacityNHUses)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.AverageContractedCapacityNHUses)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
