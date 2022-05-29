namespace Datiss.Budget.Services.Models
{
    public class CreateCostForcastPipingWsDTO
    {
        public int YearId { get; set; }

        public int TubeTypeId { get; set; }

        public int DiameterPipeTypeId { get; set; }

        public int DigTypeId { get; set; }

        public long TubeBuyCost { get; set; }

        public long NaghabCost { get; set; }

        public long TeransheCost { get; set; }


    }

    public class UpdateCostForcastPipingWsDTO : CreateCostForcastPipingWsDTO
    {
        public int Id { get; set; }

    }

    public class CostForcastPipingWsDTO
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

        public long NaghabCost { get; set; }

        public long TeransheCost { get; set; }
    }
}
