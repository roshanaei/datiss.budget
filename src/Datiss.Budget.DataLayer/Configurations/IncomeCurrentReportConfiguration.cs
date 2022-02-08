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
    public class IncomeCurrentReportConfiguration : IEntityTypeConfiguration<IncomeCurrentReport>
    {
        public void Configure(EntityTypeBuilder<IncomeCurrentReport> builder)
        {
            builder.ToTable("IncomeCurrentReports");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("IncomeCurrentReportId");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CurrentIncomeReports)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CurrentIncomeReports)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SectionType)
                    .WithMany(x => x.CurrentIncomeReports)
                    .HasForeignKey(x => x.SectionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UnitType)
                    .WithMany(x => x.UnitTypeCurrentIncomeReports)
                    .HasForeignKey(x => x.UnitTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
