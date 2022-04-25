using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class BudgetSourceReport : IAuditableEntity
    {
        public BudgetSourceReport() { }


        #region Properties

        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int SectionTypeId { get; set; }

        public long FunctionalBasicYear { get; set; }

        public long FunctionalYear_1 { get; set; }

        public long ApproveYear_1 { get; set; }

        public long ForcastY { get; set; }

        public decimal ReceiptPercent { get; set; }

        public long Fee { get; set; }

        public decimal ForcastFunctionalPercent { get; set; }

        public decimal ForcastBudgetPercent { get; set; }


        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant SectionType { get; set; }

        #endregion
    }
}
