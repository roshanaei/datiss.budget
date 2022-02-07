using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class IncomeCurrentWsH : IAuditableEntity
    {
        public IncomeCurrentWsH() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int UsageLayerId { get; set; }

        public int NumberUser { get; set; }

        public int UnitUser { get; set; }

        public decimal AvgConsumeUser { get; set; }

        public int ConsumptionUser { get; set; }

        public long Cost { get; set; }

        public long Income { get; set; }

        public long SubscriptionIncome { get; set; }

        public long Note3Price { get; set; }

        public long Note3Income { get; set; }

        public long SeasonalIncome { get; set; }
        
        public long TIncome { get; set; }
        
        public long Note7Price { get; set; }

        public long Note7Income { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        public Constant UsageLayer { get; set; }

        #endregion
    }
}
