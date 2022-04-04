using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostCurrentFinancingImportModel
    {

        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string FinancialCostTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int FinancialCostTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public long BaseFee { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public long LastYearFee { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long ForcastFee { get; set; }

    }
}
