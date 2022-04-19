using Ganss.Excel;

namespace Datiss.Budget.Services.Excel.Models
{
    public class CostCurrentRawMaterialImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string ActivityTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int ActivityType { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string RawMaterialTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int RawMaterialTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long BaseFee { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public long LastYearFee { get; set; }

    }
}
