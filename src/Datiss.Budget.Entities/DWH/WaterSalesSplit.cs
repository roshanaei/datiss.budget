using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class WaterSalesSplit :IAuditableEntity
    {
        public WaterSalesSplit() { }

        #region properties

        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int WPipeDiameterId { get; set; }

        public int NumberSales { get; set; }
        
        public int UnitSales { get; set; }

        public decimal AverageCapacity { get; set; }

        public int WInstallationCosts { get; set; }

        #endregion

        #region Navigation

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        public Constant WPipeDiameter { get; set; }

        #endregion
    }
}
