using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentSharingSetad :IAuditableEntity
    {
        public CostCurrentSharingSetad() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public decimal IncomeCurrentWSharingCoff { get; set; }

        public decimal IncomeCurrentWsSharingCoff { get; set; }

        public decimal IncomeForcastsharing { get; set; }

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        #endregion
    }
}
