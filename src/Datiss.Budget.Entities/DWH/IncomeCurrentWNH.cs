using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class IncomeCurrentWNH : IAuditableEntity
    {
        public IncomeCurrentWNH() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int NumberUser { get;set;}

        public int UnitUser { get;set;}

        public decimal AvgConsumeUser { get; set; }

        public int ConsumptionUser { get; set; }

        public decimal Capacity { get; set; }

        public int Cost { get; set; }

        public int Income { get; set; }

        public int ExcessIncome { get; set; }
        
        public int SeasonalIncome { get; set; }

        public int Note3Price { get; set; }
        
        public int Note3Income { get; set; }
        
        public int SubscriptionIncome { get; set; }

        public int TotalIncome { get; set; }

        public int Diff_ConsWsVolume { get; set; }

        public int Note2Income { get; set; }
        
        public int WasteVolume { get; set; }
        
        public int Note7Price { get; set; }
        
        public int Note7Income { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }
        #endregion
    }
}
