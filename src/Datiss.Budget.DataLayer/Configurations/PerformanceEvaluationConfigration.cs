using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class PerformanceEvaluationConfigration :IEntityTypeConfiguration<PerformanceEvaluation>
    {
        public void Configure(EntityTypeBuilder<PerformanceEvaluation> builder)
        {
            builder.ToTable("PerformanceEvaluation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Target).HasColumnType("decimal(18,6)");

            builder.Property(x => x.Operation).HasColumnType("decimal(18,6)");

            builder.Property(x => x.Status)
                    .HasDefaultValue(EntityStatus.Enabled);

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.PerformanceEvaluation)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.PerformanceEvaluation)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TablesFiled)
                    .WithMany(x => x.PerformanceEvaluation)
                    .HasForeignKey(x => x.TableFieldId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
