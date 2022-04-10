using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostCurrentWaterSourceImportModel
    {

        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string WaterSourceTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int WaterSourceTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int ActiveSource { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public long BaseProduction { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long LastYearProduction { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public long ForcastProduction { get; set; }
    }
}
