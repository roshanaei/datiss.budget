namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentWaterSourcePriceDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int WaterSourceTypeId { get; set; }

        public string WaterSourceTypeTitle { get; set; }

        public long Price { get; set; }
    }

    public class UpdateCostCurrentWaterSourcePriceDTO : CreateCostCurrentWaterSourcePriceDTO
    {
        public int Id { get; set; }

    }

    public class CostCurrentWaterSourcePriceDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int WaterSourceTypeId { get; set; }
        public string WaterSourceTypeDisplay { get; set; }
        public long Price { get; set; }
    }
}
