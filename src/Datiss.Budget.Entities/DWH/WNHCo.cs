using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class WNHCo : IAuditableEntity
    {
        public WNHCo() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int P1Capacity { get; set; }

        public int FixCostCo { get;set;}

        public int P1CostCo { get;set;}

        public int P2CostCo { get;set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        #endregion
    }
}
