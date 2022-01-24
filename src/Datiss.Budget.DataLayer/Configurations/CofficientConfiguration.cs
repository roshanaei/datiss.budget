using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class CofficientConfiguration : IEntityTypeConfiguration<Cofficient>
    {
        public void Configure(EntityTypeBuilder<Cofficient> builder)
        {
            builder.ToTable("Cofficients");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CofficientId");

            builder.Property(x => x.GroupName)
                .HasDefaultValue(CofficientsGroup.CurrentIncome);

            builder.Property(x => x.Fee).HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.FinanceYear)
                .WithMany(x => x.Cofficients)
                .HasForeignKey(x => x.YearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.Cofficients)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CofficientType)
                .WithMany(x => x.Cofficients)
                .HasForeignKey(x => x.CofficientTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
