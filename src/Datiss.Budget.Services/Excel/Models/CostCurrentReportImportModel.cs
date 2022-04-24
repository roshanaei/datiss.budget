using Ganss.Excel;

namespace Datiss.Budget.Services.Excel.Models
{
    public class CostCurrentReportImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string ActivityName { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int? Activity { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string SectionTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int SectionTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public string UnitTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int UnitTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public string UnitDetailTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public int UnitDetailTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public string CostCenterTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public long FunctionalBasicYear { get; set; }

        [Column(MappingDirections.Both, Letter = "N")]
        public long FunctionalYear_1 { get; set; }
    }
}
