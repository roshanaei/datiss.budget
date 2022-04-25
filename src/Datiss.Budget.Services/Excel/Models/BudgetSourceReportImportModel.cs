using Ganss.Excel;

namespace Datiss.Budget.Services.Excel.Models
{
    public class BudgetSourceReportImportModel
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
        public long FunctionalBasicYear { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public long FunctionalYear_1 { get; set; }
    }
}
