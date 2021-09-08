using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Configurations
{
    public class TablesFiledTitleConfiguration : IEntityTypeConfiguration<TablesFiledTitle>
    {
        public void Configure(EntityTypeBuilder<TablesFiledTitle> builder)
        {
            builder.ToTable("TablesFiledTitle");

            builder.HasKey(x => x.Id);

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
