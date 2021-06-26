using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class WaterInstallFee : IAuditableEntity
    {

        public WaterInstallFee() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int DWaterTypeId { get; set; }

        public int WInstllFee { get; set; }

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant DWaterType { get; set; }

        #endregion

     }
}