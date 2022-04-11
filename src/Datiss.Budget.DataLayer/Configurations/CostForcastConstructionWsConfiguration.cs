using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostForcastConstructionWsConfiguration : IEntityTypeConfiguration<CostForcastConstructionWs>
    {
        public void Configure(EntityTypeBuilder<CostForcastConstructionWs> builder)
        {
            builder.ToTable("CostForcastConstructionWs").HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CFCWsId");


            builder.Property(x => x.ProjectDescription)
                    .HasMaxLength(500)
                    .IsUnicode();

            builder.HasOne(x => x.FinanceYear)
                               .WithMany(x => x.CostForcastConstructionWs)
                               .HasForeignKey(x => x.YearId)
                               .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostForcastConstructionWs)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.WasteInvestors)
                    .WithMany(x => x.CostForcastConstructionWsInvestors)
                    .HasForeignKey(x => x.WasteInvestorsTypeId)
                    .OnDelete(DeleteBehavior.Restrict);



            builder.HasOne(x => x.CostCenter)
                    .WithMany(x => x.CostForcastConstructionWsCostCenters)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.ExploitationArea)
                    .WithMany(x => x.CostForcastConstructionWsExploitationArea)
                    .HasForeignKey(x => x.ExploitationAreaTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Measurement)
                    .WithMany(x => x.CostForcastConstructionWsMeasurement)
                    .HasForeignKey(x => x.MeasurementTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Credit)
                    .WithMany(x => x.CostForcastConstructionWsCredit)
                    .HasForeignKey(x => x.CreditTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Extension)
                    .WithMany(x => x.CostForcastConstructionWsExtension)
                    .HasForeignKey(x => x.ExtensionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SuggestedBudgetTopic)
                    .WithMany(x => x.CostForcastConstructionWsSuggestedBudgetTopic)
                    .HasForeignKey(x => x.SuggestedBudgetTopicTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
