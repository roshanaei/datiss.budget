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

        [Column(MappingDirections.Both, Letter = "C")]
        public int WaterInvestorsTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int ExploitationAreaTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int ProgressPercent { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public long CostDone { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int Amount { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int MeasurementTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public long UnitPrice { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public long TotalCost { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int CreditTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int ExtensionTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int SuggestedBudgetTopicTypeId { get; set; }
    }
}
