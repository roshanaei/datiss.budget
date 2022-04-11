using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostForcastConstructionWConfiguration :IEntityTypeConfiguration<CostForcastConstructionW>
    {
        public void Configure(EntityTypeBuilder<CostForcastConstructionW> builder)
        {
            builder.ToTable("CostForcastConstructionW").HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CFCWId");

            builder.Property(x => x.ProjectDescription)
                        .HasMaxLength(500)
                        .IsUnicode();


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostForcastConstructionW)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostForcastConstructionW)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.WaterInvestors)
                    .WithMany(x => x.CostForcastConstructionWInvestors)
                    .HasForeignKey(x => x.WaterInvestorsTypeId)
                    .OnDelete(DeleteBehavior.Restrict);



            builder.HasOne(x => x.CostCenter)
                    .WithMany(x => x.CostForcastConstructionWCostCenters)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.ExploitationArea)
                    .WithMany(x => x.CostForcastConstructionWExploitationArea)
                    .HasForeignKey(x => x.ExploitationAreaTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Measurement)
                    .WithMany(x => x.CostForcastConstructionWMeasurement)
                    .HasForeignKey(x => x.MeasurementTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Credit)
                    .WithMany(x => x.CostForcastConstructionWCredit)
                    .HasForeignKey(x => x.CreditTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Extension)
                    .WithMany(x => x.CostForcastConstructionWExtension)
                    .HasForeignKey(x => x.ExtensionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SuggestedBudgetTopic)
                    .WithMany(x => x.CostForcastConstructionWSuggestedBudgetTopic)
                    .HasForeignKey(x => x.SuggestedBudgetTopicTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
