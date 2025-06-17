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
        public string CostCurrentNoDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int CostCurrentNoTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public long ForcastFee { get; set; }

    }
}
