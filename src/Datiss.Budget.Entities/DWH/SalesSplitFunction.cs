using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class SalesSplitFunction : IAuditableEntity
    {
        public SalesSplitFunction() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int WNumber { get; set; }

        public int WUnit { get;set;}

        public int WsNumber { get;set;}

        public int WsUnit { get;set;}

        public int WNumber_2 { get; set; }

        public int WUnit_2 { get; set; }

        public int WsNumber_2 { get; set; }

        public int WsUnit_2 { get; set; }

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }
        #endregion
    }
}