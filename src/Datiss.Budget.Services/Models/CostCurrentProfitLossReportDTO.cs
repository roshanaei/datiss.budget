namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentProfitLossReportDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int SectionTypeId { get; set; }

        public string SectionTypeTitle { get; set; }

        public long FunctionalYear_1 { get; set; }

        public long FunctionalBasicYear { get; set; }

        public long ApproveYear_1 { get; set; }

        public long ForcastY { get; set; }
    }

    public class UpdateCostCurrentProfitLossReportDTO : CreateCostCurrentProfitLossReportDTO
    {
        public int Id { get; set; }
    }

    public class CostCurrentProfitLossReportDTO
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
    }
}
