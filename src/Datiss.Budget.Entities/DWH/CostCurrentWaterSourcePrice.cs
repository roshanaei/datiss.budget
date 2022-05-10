using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentWaterSourcePrice : IAuditableEntity
    {

        public CostCurrentWaterSourcePrice() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int WaterSourceTypeId { get; set; }

        public long Price { get; set; }

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant WaterSourceType { get; set; }

        #endregion

     }
}