using Datiss.Budget.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{

    public class ConstantConfiguration : IEntityTypeConfiguration<Constant>
    {
        public void Configure(EntityTypeBuilder<Constant> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ConstantId");

            builder.Property(x => x.Title).HasMaxLength(400).IsRequired();

            builder.HasOne(x => x.Parent)
                    .WithMany(x => x.Childrens)
                    .HasForeignKey(x => x.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Constants");
        }
    }

}