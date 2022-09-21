
namespace Datiss.Budget.Services.Models
{
    public class UpdateCostForcastConsumptionReportDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int SectionTypeId { get; set; }

        public string SectionTypeTitle { get; set; }

        public long FunctionalYear_1 { get; set; }

        public long FunctionalBasicYear { get; set; }

        public long ApproveYear_1 { get; set; }

        public long ForcastY { get; set; }

        public decimal ReceiptPercent { get; set; }

        public long Fee { get; set; }

        public decimal ForcastFunctionalPercent { get; set; }

        public decimal ForcastBudgetPercent { get; set; }

    }

    public class CostForcastConsumptionReportDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int SectionTypeId { get; set; }

        public string SectionTypeDisplay { get; set; }

        public long FunctionalYear_1 { get; set; }

        public long FunctionalBasicYear { get; set; }

        public long ApproveYear_1 { get; set; }

        public long ForcastY { get; set; }

        public decimal ReceiptPercent { get; set; }

        public long Fee { get; set; }

        public decimal ForcastFunctionalPercent { get; set; }

        public decimal ForcastBudgetPercent { get; set; }
    }
}
