using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities.DWH;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datiss.Budget.DataLayer.Mappings
{
    public class CostCurrentPersonelConfiguration :IEntityTypeConfiguration<CostCurrentPersonel>
    {
        public void Configure(EntityTypeBuilder<CostCurrentPersonel> builder)
        {
            builder.ToTable("CostCurrentPersonel")
                    .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .HasColumnName("PersonelId");

            builder.HasOne(x => x.FinanceYear)
                    .WithMany(x => x.CostCurrentPersonel)
                    .HasForeignKey(x => x.YearId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
                    .WithMany(x => x.CostCurrentPersonel)
                    .HasForeignKey(x => x.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.CostCenter)
                    .WithMany(x => x.CostCurrentPersonelCostCenter)
                    .HasForeignKey(x => x.CostCenterTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Grade)
                    .WithMany(x => x.CostCurrentPersonelGrade)
                    .HasForeignKey(x => x.GradeTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Contract)
                    .WithMany(x => x.CostCurrentPersonelContract)
                    .HasForeignKey(x => x.ContractTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.JobDepartment)
                    .WithMany(x => x.CostCurrentPersonelJobDepartment)
                    .HasForeignKey(x => x.JobDepartmentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.JobStatus)
                    .WithMany(x => x.CostCurrentPersonelJobStatus)
                    .HasForeignKey(x => x.JobStatusTypeId)
                    .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.JobStatusDetail)
                    .WithMany(x => x.CostCurrentPersonelJobStatusDetail)
                    .HasForeignKey(x => x.JobStatusDetailTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
