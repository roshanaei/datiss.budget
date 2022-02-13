using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class IncomeCurrentInstalation : IAuditableEntity
    {
        public IncomeCurrentInstalation() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ICInstalationTypeId { get; set; }

        public int NumberUser { get; set; }

        public int Cost { get; set; }

        public long Income { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant ICInstalationType { get; set; }

        #endregion
    }
}
