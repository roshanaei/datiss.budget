using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{

    public class ConstantConfiguration : IEntityTypeConfiguration<Constant>
    {
        public void Configure(EntityTypeBuilder<Constant> builder)
        {
            builder.ToTable("Constants");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ConstantId");

            builder.Property(x => x.Title)
                .HasMaxLength(400)
                .IsUnicode()
                .IsRequired();

            builder.Property(x => x.ConstantKey)
                .HasMaxLength(50)
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