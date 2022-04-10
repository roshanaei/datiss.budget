using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostCurrentOtherImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string CostCenterTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string CCOtherCostsTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int CCOtherCostsTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long BaseFee { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public long LastYearFee { get; set; }

    }
}
