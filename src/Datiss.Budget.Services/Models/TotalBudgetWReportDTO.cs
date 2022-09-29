
namespace Datiss.Budget.Services.Models
{
    public class UpdateTotalBudgetWReportDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int SectionTypeId { get; set; }

        public string SectionTypeTitle { get; set; }

        public int UnitTypeId { get; set; }

        public string UnitTypeTitle { get; set; }   

        public long FunctionalYear_1 { get; set; }

        public long FunctionalBasicYear { get; set; }

        public long ApproveYear_1 { get; set; }

        public long ForcastY { get; set; }

        public decimal ForcastFunctionalPercent { get; set; }

        public decimal ForcastBudgetPercent { get; set; }

    }

    public class TotalBudgetWReportDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int SectionTypeId { get; set; }

        public string SectionTypeDisplay { get; set; }

        public int UnitTypeId { get; set; }

        public string UnitTypeDisplay { get; set; }

        public long FunctionalYear_1 { get; set; }

        public long FunctionalBasicYear { get; set; }

        public long ApproveYear_1 { get; set; }

        public long ForcastY { get; set; }

        public decimal ForcastFunctionalPercent { get; set; }

        public decimal ForcastBudgetPercent { get; set; }
    }
}
