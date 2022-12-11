namespace Datiss.Budget.Services.Models
{
    public class CreateCostForcastWsInvestmentReportDTO
    {
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeTitle { get; set; }
        public int SectionTypeId { get; set; }
        public string SectionTypeTitle { get; set; }
        public int UnitTypeId { get; set; }
        public string UnitTypeTitle { get; set; }
        public int Amount1 { get; set; }
        public long Cost1 { get; set; }
        public int Amount2 { get; set; }
        public long Cost2 { get; set; }
        public int Amount3 { get; set; }
        public long Cost3 { get; set; }
        public int Amount4 { get; set; }
        public long Cost4 { get; set; }
    }
    public class UpdateCostForcastWsInvestmentReportDTO : CreateCostForcastWsInvestmentReportDTO
    {
        public int Id { get; set; }
    }

    public class CostForcastWsInvestmentReportDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }
        public int SectionTypeId { get; set; }
        public string SectionTypeDisplay { get; set; }
        public int UnitTypeId { get; set; }
        public string UnitTypeDisplay { get; set; }
        public int Amount1 { get; set; }
        public long Cost1 { get; set; }
        public int Amount2 { get; set; }
        public long Cost2 { get; set; }
        public int Amount3 { get; set; }
        public long Cost3 { get; set; }
        public int Amount4 { get; set; }
        public long Cost4 { get; set; }
    }
}
