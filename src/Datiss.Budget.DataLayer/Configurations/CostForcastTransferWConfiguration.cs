using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostForcastTransferWConfiguration :IEntityTypeConfiguration<CostForcastTransferW>
    {
        public void Configure(EntityTypeBuilder<CostForcastTransferW> builder)
        {
            builder.ToTable("CostForcastTransferW")
                    .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CFCTWId");

            builder.Property(x => x.Location)
                .HasMaxLength(300)
                .IsUnicode();

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostForcastTransferW)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostForcastTransferW)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TransferType)
                     .WithMany(x => x.CostForcastTransferWTransfer)
                     .HasForeignKey(x => x.TransferTypeId)
                     .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Credit)
                     .WithMany(x => x.CostForcastTransferWCredit)
                     .HasForeignKey(x => x.CreditTypeId)
                     .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.DigType)
                     .WithMany(x => x.CostForcastTransferWDig)
                     .HasForeignKey(x => x.DigTypeId)
                     .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.TubeType)
                     .WithMany(x => x.CostForcastTransferWTube)
                     .HasForeignKey(x => x.TubeTypeId)
                     .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.DiameterType)
                    .WithMany(x => x.CostForcastTransferWDiameterPipe)
                    .HasForeignKey(x => x.DiameterPipeTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Extension)
                     .WithMany(x => x.CostForcastTransferWExtension)
                     .HasForeignKey(x => x.ExtensionTypeId)
                     .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.SuggestedBudgetTopic)
                    .WithMany(x => x.CostForcastTransferWSuggestedBudgetTopic)
                    .HasForeignKey(x => x.SuggestedBudgetTopicTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
