namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentContractualDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CostCenterTypeId { get; set; }

        public string CostCenterTypeTitle { get; set; }

        public string ContractDescription { get; set; }

        public bool ExtensionId { get; set; }

        public string ExtensionTitle { get; set; }

        public long ContractLastYear { get; set; }

        public long ContractForcast { get; set; }

    }

    public class UpdateCostCurrentContractualDTO : CreateCostCurrentContractualDTO
    {
        public int Id { get; set; }

    }

    public class CostCurrentContractualDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }
        public string ContractDescription { get; set; }
        public bool ExtensionId { get; set; }
        public string ExtensionDisplay { get; set; }
        public long ContractLastYear { get; set; }
        public long ContractForcast { get; set; }
    }
}
