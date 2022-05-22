using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostForcastPipingWImportModel
    {

        [Column(MappingDirections.Both, Letter = "A")]
        public int TubeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int DiameterPipeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int DigTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public long TubeBuyCost { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public long RunCost { get; set; }

    }
}
