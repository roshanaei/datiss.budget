using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class IncomeForcastSourcesReportConfiguration : IEntityTypeConfiguration<IncomeForcastSourcesReport>
    {
        public void Configure(EntityTypeBuilder<IncomeForcastSourcesReport> builder)
        {
            builder.ToTable("IncomeForcastSourcesReport");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("IFSRId");

            builder.HasOne(x => x.FinanceYear)
                .WithMany(x => x.IncomeForcastSourcesReport)
                .HasForeignKey(x => x.YearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.IncomeForcastSourcesReport)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SourceDescription)
                .WithMany(x => x.IncomeForcastSourcesReport)
                .HasForeignKey(x => x.SourceDescriptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
