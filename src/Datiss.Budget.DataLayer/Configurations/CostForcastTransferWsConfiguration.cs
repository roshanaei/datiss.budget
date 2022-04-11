using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostForcastTransferWsConfiguration :IEntityTypeConfiguration<CostForcastTransferWs>
    {
        public void Configure(EntityTypeBuilder<CostForcastTransferWs> builder)
        {
            builder.ToTable("CostForcastTransferWs")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CFTWsId");

            builder.Property(x => x.Location)
                    .HasMaxLength(300)
                    .IsUnicode();


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostForcastTransferWs)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostForcastTransferWs)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TransferType)
                     .WithMany(x => x.CostForcastTransferWsTransfer)
                     .HasForeignKey(x => x.TransferTypeId)
                     .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Credit)
                     .WithMany(x => x.CostForcastTransferWsCreadit)
                     .HasForeignKey(x => x.CreditTypeId)
                     .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.DigType)
                     .WithMany(x => x.CostForcastTransferWsDig)
                     .HasForeignKey(x => x.DigTypeId)
                     .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.MethodType)
                       .WithMany(x => x.CostForcastTransferWsMethod)
                       .HasForeignKey(x => x.MethodTypeId)
                       .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TubeType)
                     .WithMany(x => x.CostForcastTransferWsTube)
                     .HasForeignKey(x => x.TubeTypeId)
                     .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.DiameterType)
                    .WithMany(x => x.CostForcastTransferWsDiameterPipe)
                    .HasForeignKey(x => x.DiameterPipeTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Extension)
                     .WithMany(x => x.CostForcastTransferWsExtension)
                     .HasForeignKey(x => x.ExtensionTypeId)
                     .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.SuggestedBudgetTopic)
                    .WithMany(x => x.CostForcastTransferWsSuggestedBudgetTopic)
                    .HasForeignKey(x => x.SuggestedBudgetTopicTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
