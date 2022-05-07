namespace Datiss.Budget.Services.Models
{
    public class CreateCostForcastFinanceDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeTitle { get; set; }

        public int FinanceSubjectTypeId { get; set; }
        public string FinanceSubjectTypeTitle { get; set; }

        public long RemainingAssets { get; set; }

        public long AssetsCreated6_1 { get; set; }

        public long AssetsCreated6_2 { get; set; }

        public long ForcastAssets_D { get; set; }

        public long TotalAssetsCreated_D { get; set; }
    }

    public class UpdateCostForcastFinanceDTO : CreateCostForcastFinanceDTO
    {
        public int Id { get; set; }

    }

    public class CostForcastFinanceDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }
        public int FinanceSubjectTypeId { get; set; }
        public string FinanceSubjectTypeDisplay { get; set; }
        public long RemainingAssets { get; set; }
        public long AssetsCreated6_1 { get; set; }
        public long AssetsCreated6_2 { get; set; }
        public long ForcastAssets_D { get; set; }
        public long TotalAssetsCreated_D { get; set; }
    }

}
