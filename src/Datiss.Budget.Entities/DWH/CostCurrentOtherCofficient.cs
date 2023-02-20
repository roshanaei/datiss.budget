using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentOtherCofficient : IAuditableEntity
    {
        public CostCurrentOtherCofficient() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int CCOtherCostsTypeId { get; set; }

        public long ForcastFee { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Constant CostCenter { get; set; }

        public Constant CCOtherCosts { get; set; }
        #endregion
    }
}
