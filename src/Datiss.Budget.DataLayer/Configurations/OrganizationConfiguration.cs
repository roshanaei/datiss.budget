using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{

    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("Organizations").HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("OrganizationId");

            builder.Property(x => x.Type).HasColumnName("OrgType");

            builder.Property(x => x.Title)
                .HasMaxLength(400)
                .IsUnicode()
                .IsRequired();

            builder.Property(x => x.Status)
                .HasDefaultValue(EntityStatus.Enabled);

            builder.HasOne(x => x.Parent)
                    .WithMany(x => x.Childrens)
                    .HasForeignKey(x => x.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);

            
        }
    }

}