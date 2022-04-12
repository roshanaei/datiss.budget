using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class CostForcastBuy : IAuditableEntity
    {
        public CostForcastBuy() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int  OrganizationId { get; set; }

        public string BuyDescription { get; set; }  

        public int BuyOrganizationId { get; set; }

        public int CostCenterTypeId { get; set; }


        #endregion

        #region Navigation
        #endregion
    }
}
