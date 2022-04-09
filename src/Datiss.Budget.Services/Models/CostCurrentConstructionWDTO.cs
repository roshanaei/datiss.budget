namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentConstructionWDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public string ProjectDescription { get; set; }

        public int WaterInvestorsTypeId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int ExploitationAreaTypeId { get; set; }

        public int ProgressPercent { get; set; }

        public long CostDone { get; set; }

        public int Amount { get; set; }

        public int MeasurementTypeId { get; set; }

        public long UnitPrice { get; set; }

        public long TotalCost { get; set; }

        public int CreditTypeId { get; set; }

        public int ExtensionTypeId { get; set; }

        public int SuggestedBudgetTopicTypeId { get; set; }

    }

    public class UpdateCostCurrentConstructionWDTO : CreateCostCurrentConstructionWDTO
    {
        public int Id { get; set; }

    }

    public class CostCurrentConstructionWDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }

        public int Year { get; set; }
        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }
        public string ProjectDescription { get; set; }

        public int WaterInvestorsTypeId { get; set; }
        public string WaterInvestorsDisplay { get; set; }

        public int CostCenterTypeId { get; set; }
        public string CostCenterDisplay { get; set; }

        public int ExploitationAreaTypeId { get; set; }
        public string ExploitationAreaDisplay { get; set; }

        public int ProgressPercent { get; set; }

        public long CostDone { get; set; }

        public int Amount { get; set; }

        public int MeasurementTypeId { get; set; }
        public string MeasurementDisplay { get; set; }

        public long UnitPrice { get; set; }

        public long TotalCost { get; set; }

        public int CreditTypeId { get; set; }
        public string CreditDisplay { get; set; }

        public int ExtensionTypeId { get; set; }
        public string ExtensionDisplay { get; set; }

        public int SuggestedBudgetTopicTypeId { get; set; }
        public string SuggestedBudgetTopicDisplay { get; set; }
    }
}
