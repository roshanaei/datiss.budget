using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;


namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentBankFee : IAuditableEntity
    {
        public CostCurrentBankFee() { }
        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CostCenterTypeId { get; set; }

        public long BankFeeLastYear { get; set; }

        public long BankFeeForcast { get; set; }
        #endregion

        #region navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant CostCenterType { get; set; }
        #endregion
    }
}
