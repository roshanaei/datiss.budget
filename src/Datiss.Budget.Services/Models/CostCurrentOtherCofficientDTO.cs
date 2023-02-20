namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentOtherCofficientDTO
    {
        public int YearId { get; set; }

        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeTitle { get; set; }

        public int CCOtherCostsTypeId { get; set; }
        public string CCOtherCostsTypeTitle { get; set; }

        public decimal Fee { get; set; }
    }

    public class UpdateCostCurrentOtherCofficientDTO : CreateCostCurrentOtherCofficientDTO
    {
        public int Id { get; set; }

    }

    public class CostCurrentOtherCofficientDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }
        public int CCOtherCostsTypeId { get; set; }
        public string CCOtherCostsTypeDisplay { get; set; }
        public decimal Fee { get; set; }

    }

}
