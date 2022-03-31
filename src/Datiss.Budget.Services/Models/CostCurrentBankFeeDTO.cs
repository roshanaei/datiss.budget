namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentBankFeeDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CostCenterTypeId { get; set; }

        public string CostCenterTypeTitle { get; set; }

        public long BankFeeLastYear { get; set; }

        public long BankFeeForcast { get; set; }

    }

    public class UpdateCostCurrentBankFeeDTO : CreateCostCurrentBankFeeDTO
    {
        public int Id { get; set; }

    }

    public class CostCurrentBankFeeDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }
        public long BankFeeLastYear { get; set; }
        public long BankFeeForcast { get; set; }
    }
}
