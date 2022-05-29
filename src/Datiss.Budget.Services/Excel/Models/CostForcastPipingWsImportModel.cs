using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostForcastPipingWsImportModel
    {

        [Column(MappingDirections.Both, Letter = "A")]
        public string TubeTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int TubeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string DiameterPipeTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int DiameterPipeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string DigTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int DigTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long TubeBuyCost { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public long NaghabCost { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public long TeransheCost { get; set; }

    }
}
