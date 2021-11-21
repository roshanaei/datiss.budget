using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class IncomeCurrentOperational :IAuditableEntity
    {
        public IncomeCurrentOperational() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ICOTypeId { get; set; }

        public int CountH { get; set; }

        public int PriceH { get; set; }

        public int CostH { get; set; }

        public int CountNH { get; set; }

        public int PriceNH { get; set; }

        public int CostNH { get; set; }

        public int TotalCount { get; set; }

        public int TotalCost { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant ICOType { get; set; }
        #endregion
    }
}