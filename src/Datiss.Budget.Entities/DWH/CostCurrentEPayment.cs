using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentEPayment : IAuditableEntity
    {
        public CostCurrentEPayment() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int BillingCycle { get; set; }

        public decimal EPayForcast { get; set; }

        public long EPayBFee { get; set; }

        public decimal PPayForcast { get; set; }

        public long PPayBFee { get; set; }
        #endregion

        #region navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }
        #endregion
    }
}
