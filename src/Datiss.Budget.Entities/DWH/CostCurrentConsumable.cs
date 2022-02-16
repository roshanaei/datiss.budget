using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentConsumable :IAuditableEntity
    {
        public CostCurrentConsumable() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ConsumableTypeId { get; set; }

        public int ConsumableAmount { get; set; }

        public long ConsumableCost { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant ConsumableType { get; set; }

        #endregion
    }
}
