namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentWaterSourceDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int WaterSourceTypeId { get; set; }

        public string WaterSourceTypeTitle { get; set; }

        public int ActiveSource { get; set; }

        public long BaseProduction { get; set; }

        public long LastYearProduction { get; set; }

        public long ForcastProduction { get; set; }

    }

    public class UpdateCostCurrentWaterSourceDTO : CreateCostCurrentWaterSourceDTO
    {
        public int Id { get; set; }

    }

    public class CostCurrentWaterSourceDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int WaterSourceTypeId { get; set; }

        public string WaterSourceTypeDisplay { get; set; }

        public int ActiveSource { get; set; }

        public long BaseProduction { get; set; }

        public long LastYearProduction { get; set; }

        public long ForcastProduction { get; set; }

    }
}
