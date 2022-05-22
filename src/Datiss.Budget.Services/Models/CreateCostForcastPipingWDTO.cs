namespace Datiss.Budget.Services.Models
{
    public class CreateCostForcastPipingWDTO
    {
        public int YearId { get; set; }

        public int TubeTypeId { get; set; }

        public int DiameterPipeTypeId { get; set; }

        public int DigTypeId { get; set; }

        public long TubeBuyCost { get; set; }

        public long RunCost { get; set; }

    }

    public class UpdateCostForcastPipingWDTO : CreateCostForcastPipingWDTO
    {
        public int Id { get; set; }

    }

    public class CostForcastPipingWDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int TubeTypeId { get; set; }
        public string TubeTypeDisplay { get; set; }

        public int DiameterPipeTypeId { get; set; }
        public string DiameterPipeTypeDisplay { get; set; }

        public int DigTypeId { get; set; }
        public string DigTypeDisplay { get; set; }

        public long TubeBuyCost { get; set; }

        public long RunCost { get; set; }

        public long TotalCost { get; set; }

    }
}
