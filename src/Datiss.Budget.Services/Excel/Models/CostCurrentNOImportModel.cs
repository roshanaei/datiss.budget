using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostCurrentNOImportModel
    {

        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string CostCurrentNOTitle { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int CostCurrentNOTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public long BaseFee { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public long LastYearFee { get; set; }
    }
}
