namespace Datiss.Budget.Services.Models
{
    public class CreateCostForcastBuyDescriptionDTO
    {
        public int YearId { get; set; }

        public int AssetTypeId { get; set; }

        public int AssetDetailTypeId { get; set; }

        public int MeasurementTypeId { get; set; }

        public long UnitPrice { get; set; }

    }

    public class UpdateCostForcastBuyDescriptionDTO : CreateCostForcastBuyDescriptionDTO
    {
        public int Id { get; set; }

    }

    public class CostForcastBuyDescriptionDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int AssetTypeId { get; set; }
        public string AssetTypeDisplay { get; set; }

        public int AssetDetailTypeId { get; set; }
        public string AssetDetailTypeDisplay { get; set; }

        public int MeasurementTypeId { get; set; }
        public string MeasurementTypeDisplay { get; set; }

        public long UnitPrice { get; set; }

    }
}
