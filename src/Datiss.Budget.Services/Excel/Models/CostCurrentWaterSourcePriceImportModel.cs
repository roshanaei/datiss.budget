using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostCurrentWaterSourcePriceImportModel
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
        public long Price { get; set; }

    }
}
