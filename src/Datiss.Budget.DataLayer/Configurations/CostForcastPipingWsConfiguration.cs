using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostForcastPipingWsConfiguration :IEntityTypeConfiguration<CostForcastPipingWs>
    {
        public void Configure(EntityTypeBuilder<CostForcastPipingWs> builder)
        {
            builder.ToTable("CostForcastPipingWs")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CFPWsId");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostForcastPipingWs)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TubeType)
                    .WithMany(x => x.CostForcastPipingWsTubeType)
                    .HasForeignKey(x => x.TubeTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DiameterPipeType)
                    .WithMany(x => x.CostForcastPipingWsDiameterPipeType)
                    .HasForeignKey(x => x.DiameterPipeTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DigType)
                .WithMany(x => x.CostForcastPipingWsDigType)
                .HasForeignKey(x => x.DigTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
