using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentConstructionWConfiguration :IEntityTypeConfiguration<CostCurrentConstructionW>
    {
        public void Configure(EntityTypeBuilder<CostCurrentConstructionW> builder)
        {
            builder.ToTable("CostCurrentConstructionW").HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("CCCWId");

            builder.Property(x => x.ProjectDescription)
                        .HasMaxLength(500)
                        .IsUnicode();


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentConstructionW)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentConstructionW)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.WaterInvestors)
                    .WithMany(x => x.CostCurrentConstructionWInvestors)
                    .HasForeignKey(x => x.WaterInvestorsTypeId)
                    .OnDelete(DeleteBehavior.Restrict);



            builder.HasOne(x => x.CostCenter)
                    .WithMany(x => x.CostCurrentConstructionWCostCenters)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.ExploitationArea)
                    .WithMany(x => x.CostCurrentConstructionWExploitationArea)
                    .HasForeignKey(x => x.ExploitationAreaTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Measurement)
                    .WithMany(x => x.CostCurrentConstructionWMeasurement)
                    .HasForeignKey(x => x.MeasurementTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Credit)
                    .WithMany(x => x.CostCurrentConstructionWCredit)
                    .HasForeignKey(x => x.CreditTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Extension)
                    .WithMany(x => x.CostCurrentConstructionWExtension)
                    .HasForeignKey(x => x.ExtensionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SuggestedBudgetTopic)
                    .WithMany(x => x.CostCurrentConstructionWSuggestedBudgetTopic)
                    .HasForeignKey(x => x.SuggestedBudgetTopicTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
