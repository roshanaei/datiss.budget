namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentFinancingDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int FinancialCostTypeId { get; set; }

        public long BaseFee { get; set; }

        public long LastYearFee { get; set; }

        public string FinancialCostTypeTitle { get; set; }
    }

    public class UpdateCostCurrentFinancingDTO : CreateCostCurrentFinancingDTO
    {
        public int Id { get; set; }
        public long ForcastFee { get; set; }

    }

    public class CostCurrentFinancingDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int FinancialCostTypeId { get; set; }
        public string FinancialCostTypeDisplay { get; set; }
        public long BaseFee { get; set; }
        public long LastYearFee { get; set; }
        public long ForcastFee { get; set; }
    }
}
