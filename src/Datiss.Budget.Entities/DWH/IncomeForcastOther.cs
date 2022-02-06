using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class IncomeForcastOther : IAuditableEntity
    {
        public IncomeForcastOther() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId {get;set;}

        public ActivityType ActivityId { get; set; }

        public int OIFTypeId { get; set; }

        public int OIFCount { get; set; }

        public long OIFPrice { get; set; }

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant OIFType { get; set; }
        #endregion
    }
}
