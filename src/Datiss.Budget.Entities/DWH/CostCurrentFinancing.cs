using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentFinancing :IAuditableEntity
    {
        public CostCurrentFinancing() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int FinancialCostTypeId { get; set; }

        public long BaseFee { get; set; }

        public long LastYearFee { get; set; }

        public long ForcastFee { get; set; }

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }  

        public Constant FinancialCostType { get; set; }

        #endregion
    }
}
