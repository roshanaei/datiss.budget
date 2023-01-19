using Ganss.Excel;

namespace Datiss.Budget.Services.Excel.Models
{
    public class DirectoratReportImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string SectionTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int? SectionTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public long ForcastY { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public long FunctionalBasicYear { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long FunctionalYear_1 { get; set; }
    }
}
