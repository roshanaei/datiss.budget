using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class ConsumeForcastConfiguration : IEntityTypeConfiguration<ConsumeForcast>
    {
        public void Configure(EntityTypeBuilder<ConsumeForcast> builder)
        {
            builder.ToTable("ConsumeForcast").HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("ConsumeForcastId");

            builder.Property(x => x.CountUser).HasColumnType("decimal(18,6)");

            builder.Property(x => x.UnitUser).HasColumnType("decimal(18,6)");

            builder.Property(x => x.ConsumeUser).HasColumnType("decimal(18,6)");

            builder.Property(x => x.AvgConsumeUser).HasColumnType("decimal(18,6)");

            builder.Property(x => x.ConsumeUserForcast).HasColumnType("decimal(18,6)");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.ConsumeForcast)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.ConsumeForcast)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.ConsumeForcast)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsageLayer)
                    .WithMany(x => x.UsageLayerConsumeForcast)
                    .HasForeignKey(x => x.UsageLayerId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
