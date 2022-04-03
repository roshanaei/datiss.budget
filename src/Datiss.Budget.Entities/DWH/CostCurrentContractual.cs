using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentContractual : IAuditableEntity
    {
        public CostCurrentContractual() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CostCenterTypeId { get; set; }

        public string ContractDescription { get; set; }

        public bool ExtensionId { get; set; }

        public long ContractLastYear { get; set; }

        public long ContractForcast { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant CostCenterType { get; set; }

        #endregion
    }
}
