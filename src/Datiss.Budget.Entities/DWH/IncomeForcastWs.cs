using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class IncomeForcastWs : IAuditableEntity
    {
        public IncomeForcastWs() { }

        #region Properties
            
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int NumberUser { get; set; }
        
        public int UnitUser { get; set; }

        public long WasteInstallIncome { get; set; }

        public long WasteBranchIncome { get; set; }

        public long WasteNote3Income { get; set; }

        public long WsNote11Income { get; set; }

        #endregion

        #region Properties

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }
        #endregion
    }
}
