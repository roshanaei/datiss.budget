using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentRawMaterial :IAuditableEntity
    {
        public CostCurrentRawMaterial() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }  

        public int RawMaterialTypeId { get; set; }

        public long BaseFee { get; set; }

        public long LastYearFee { get; set; }

        public long ForcastFee { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant RawMaterial { get; set; }
        #endregion
    }
}
