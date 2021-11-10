using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class  WWsFeeConfiguration : IEntityTypeConfiguration<WWsFee>
    {
        public void Configure(EntityTypeBuilder<WWsFee> builder)
        {
            builder.ToTable("WWsFee").HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("WWsFeeId");


            builder.Property(x => x.YearId)
                    .IsRequired();


            builder.Property(x => x.OrganizationId)
                    .IsRequired();


            builder.Property(x => x.UserTypeId)
                    .IsRequired();

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.WWsFees)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.WWsFees)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UserType)
                    .WithMany(x => x.WWsFees)
                    .HasForeignKey(x => x.UserTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
