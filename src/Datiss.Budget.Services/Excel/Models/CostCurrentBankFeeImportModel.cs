using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostCurrentBankFeeImportModel
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
        public long BankFeeLastYear { get; set; }

    }
}
