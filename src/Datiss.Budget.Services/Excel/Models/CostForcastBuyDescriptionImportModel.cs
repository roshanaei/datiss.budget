using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostForcastBuyDescriptionImportModel
    {

        [Column(MappingDirections.Both, Letter = "A")]
        public string AssetTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int AssetTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string AssetDetailTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int AssetDetailTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int MeasurementTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public long UnitPrice { get; set; }

    }
}
