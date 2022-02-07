using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder) {
            builder.ToTable("Reports").HasKey(_ => _.Id);

            builder.Property(_ => _.Name).HasMaxLength(100).IsUnicode().IsRequired();
            builder.Property(_ => _.Title).HasMaxLength(255).IsUnicode().IsRequired();
            builder.Property(_ => _.Description).HasMaxLength(500).IsUnicode();
            builder.Property(_ => _.Status).HasDefaultValue(EntityStatus.Enabled);
            
        }
    }

    public class ReportParamConfiguration : IEntityTypeConfiguration<ReportParam>
    {
        public void Configure(EntityTypeBuilder<ReportParam> builder) {
            builder.ToTable("ReportParams").HasKey(_ => _.Id);

            builder.Property(_ => _.Name).HasMaxLength(100).IsUnicode().IsRequired();
            builder.Property(_ => _.Title).HasMaxLength(255).IsUnicode();
            builder.Property(_ => _.ConstantKey).HasMaxLength(100).IsUnicode();

            builder.HasOne(_ => _.Report)
                .WithMany(_ => _.Params)
                .HasForeignKey(_ => _.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
