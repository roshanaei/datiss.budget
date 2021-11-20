using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class ConsumeForcastWsConfiguration : IEntityTypeConfiguration<ConsumeForcastWs>
    {
        public void Configure(EntityTypeBuilder<ConsumeForcastWs> builder)
        {
            builder.ToTable("ConsumeForcastWs").HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("ConsumeForcastWsId");

            builder.Property(x => x.CountUser).HasColumnType("decimal(18,6)");

            builder.Property(x => x.UnitUser).HasColumnType("decimal(18,6)");

            builder.Property(x => x.ConsumeUser).HasColumnType("decimal(18,6)");

            builder.Property(x => x.AvgConsumeUser).HasColumnType("decimal(18,6)");

            builder.Property(x => x.ConsumeUserForcast).HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.FinanceYear)
                                .WithMany(x => x.ConsumeForcastWs)
                                .HasForeignKey(x => x.YearId)
                                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.ConsumeForcastWs)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.ConsumeForcastWs)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsageLayer)
                    .WithMany(x => x.UsageLayerConsumeForcastWs)
                    .HasForeignKey(x => x.UsageLayerId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
