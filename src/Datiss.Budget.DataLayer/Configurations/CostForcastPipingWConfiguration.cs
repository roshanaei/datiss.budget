using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostForcastPipingWConfiguration :IEntityTypeConfiguration<CostForcastPipingW>
    {
        public void Configure(EntityTypeBuilder<CostForcastPipingW> builder)
        {
            builder.ToTable("CostForcastPipingW")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CFPWId");


            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostForcastPipingW)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TubeType)
                    .WithMany(x => x.CostForcastPipingWTubeType)
                    .HasForeignKey(x => x.TubeTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DiameterPipeType)
                    .WithMany(x => x.CostForcastPipingWDiameterPipeType)
                    .HasForeignKey(x => x.DiameterPipeTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DigType)
                .WithMany(x => x.CostForcastPipingWDigType)
                .HasForeignKey(x => x.DigTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
