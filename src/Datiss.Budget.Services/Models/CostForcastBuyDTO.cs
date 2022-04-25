namespace Datiss.Budget.Services.Models
{
    public class CreateCostForcastBuyDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public string BuyDescription { get; set; }

        public int LocationId { get; set; }

        public int BuyDepartmentId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int AssetTypeId { get; set; }

        public int AssetDetailTypeId { get; set; }

        public int Amount { get; set; }

        public int MeasurementTypeId { get; set; }

        public long UnitPrice { get; set; }

        public int CreditTypeId { get; set; }

        public long ProposedCost { get; set; }

    }

    public class UpdateCostForcastBuyDTO : CreateCostForcastBuyDTO
    {
        public int Id { get; set; }

    }

    public class CostForcastBuyDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public string BuyDescription { get; set; }

        public int LocationId { get; set; }
        public string LocationDisplay { get; set; }

        public int BuyDepartmentId { get; set; }
        public string BuyDepartmentDisplay { get; set; }

        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }

        public int AssetTypeId { get; set; }
        public string AssetTypeDisplay { get; set; }

        public int AssetDetailTypeId { get; set; }
        public string AssetDetailTypeDisplay { get; set; }

        public int Amount { get; set; }

        public int MeasurementTypeId { get; set; }
        public string MeasurementTypeDisplay { get; set; }

        public long UnitPrice { get; set; }

        public int CreditTypeId { get; set; }
        public string CreditTypeDisplay { get; set; }

        public long ProposedCost { get; set; }
    }
}