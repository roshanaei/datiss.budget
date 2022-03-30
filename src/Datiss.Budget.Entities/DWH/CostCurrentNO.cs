using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentNO : IAuditableEntity
    {
        public CostCurrentNO() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CostCurrentNoType { get; set; }

        public long BaseFee { get; set; }

        public long LastYearFee { get; set; }

        public long ForcastFee { get; set; }

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }  

        public Constant Constant { get; set; }

        #endregion
    }
}
