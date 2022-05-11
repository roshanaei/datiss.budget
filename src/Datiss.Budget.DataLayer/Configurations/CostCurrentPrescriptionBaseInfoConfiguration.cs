using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
        public class CostCurrentPrescriptionBaseInfoConfiguration : IEntityTypeConfiguration<CostCurrentPrescriptionBaseInfo>
        {
            public void Configure(EntityTypeBuilder<CostCurrentPrescriptionBaseInfo> builder)
            {
                builder.ToTable("CostCurrentPrescriptionBaseInfo");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id)
                        .HasColumnName("CostCurrentPrescriptionBaseInfoId");

                builder.HasOne(x => x.FinanceYear)
                        .WithMany(x => x.CostCurrentPrescriptionBaseInfo)
                        .HasForeignKey(x => x.YearId)
                        .OnDelete(DeleteBehavior.Restrict);
            }
        }
    
}
