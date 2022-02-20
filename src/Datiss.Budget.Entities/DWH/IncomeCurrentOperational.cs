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

        public long PriceH { get; set; }

        public long CostH { get; set; }

        public int CountNH { get; set; }

        public long PriceNH { get; set; }

        public long CostNH { get; set; }

        public int TotalCount { get; set; }

        public long TotalCost { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant ICOType { get; set; }
        #endregion
    }
}