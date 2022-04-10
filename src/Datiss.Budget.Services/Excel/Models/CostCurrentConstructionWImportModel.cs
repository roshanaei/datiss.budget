using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostCurrentConstructionWImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string ProjectDescription { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int WaterInvestorsTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int ExploitationAreaTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int ProgressPercent { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public long CostDone { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int MeasurementTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public long UnitPrice { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public int Amount { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public long TotalCost { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public int CreditTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "N")]
        public int ExtensionTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "O")]
        public int SuggestedBudgetTopicTypeId { get; set; }
    }
}
