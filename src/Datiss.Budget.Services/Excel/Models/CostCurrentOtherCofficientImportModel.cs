using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostCurrentOtherCofficientImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string CostCenterTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string CCOtherCostsTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int CCOtherCostsTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public decimal Fee { get; set; }

    }
}
