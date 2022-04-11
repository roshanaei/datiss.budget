using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class CostForcastConstructionWs :    IAuditableEntity
    {
        public CostForcastConstructionWs() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public string ProjectDescription { get; set; }

        public int WasteInvestorsTypeId { get; set; }

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

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant WasteInvestors { get; set; }

        public Constant CostCenter { get; set; }

        public Constant ExploitationArea { get; set; }

        public Constant Measurement { get; set; }

        public Constant Credit { get; set; }

        public Constant Extension { get; set; }

        public Constant SuggestedBudgetTopic { get; set; }
        #endregion
    }
}
