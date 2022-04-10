namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentOtherDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeTitle { get; set; }

        public int CCOtherCostsTypeId { get; set; }
        public string CCOtherCostsTypeTitle { get; set; }

        public long BaseFee { get; set; }

        public long LastYearFee { get; set; }
    }

    public class UpdateCostCurrentOtherDTO : CreateCostCurrentOtherDTO
    {
        public int Id { get; set; }
        public long ForcastFee { get; set; }

    }

    public class CostCurrentOtherDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }
        public int CCOtherCostsTypeId { get; set; }
        public string CCOtherCostsTypeDisplay { get; set; }
        public long BaseFee { get; set; }
        public long LastYearFee { get; set; }
        public long ForcastFee { get; set; }
    }

}
