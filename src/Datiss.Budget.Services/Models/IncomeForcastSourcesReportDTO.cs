namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeForcastSourcesReportDTO
    {
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public int SourceDescriptionId { get; set; }
        public string SourceDescriptionTitle { get; set; }
        public long FunctionalBasicYear { get; set; }
        public long FunctionalLastYear { get; set; }
        public long ApproveYear_1 { get; set; }
        public long PercentBudget { get; set; }
        public long ForcastY { get; set; }
    }

    public class UpdateIncomeForcastSourcesReportDTO : CreateIncomeForcastSourcesReportDTO
    {
        public int Id { get;set; }
    }

    public class IncomeForcastSourcesReportDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int SourceDescriptionId { get; set; }
        public string SourceDescriptionTitle { get; set; }
        public long FunctionalBasicYear { get; set; }
        public long FunctionalLastYear { get; set; }
        public long ApproveYear_1 { get; set; }
        public long PercentBudget { get; set; }
        public long ForcastY { get; set; }
    }
}


