using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public  class WasteSalesSplit:IAuditableEntity
    {
        public WasteSalesSplit() { }

        #region Properties
         
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set;}

        public int UserTypeId { get; set; }

        public int WsPipeDiameterId { get; set; }

        public int NumberSales { get; set; }

        public int UnitSales { get; set; }

        public decimal AverageCapacity { get; set; }

        public long WsInstallationCosts { get; set; }
        #endregion

        #region Navigation
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        public Constant WsPipeDiameter { get; set; }
        #endregion
    }
}
