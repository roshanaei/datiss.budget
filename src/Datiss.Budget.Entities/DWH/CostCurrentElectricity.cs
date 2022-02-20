using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
   public class CostCurrentElectricity : IAuditableEntity
    {
        public CostCurrentElectricity() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }
        
        public int ElectricityAmount { get; set; }

        public long ElectricityCost { get; set; }

        #endregion

        #region Navigation
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        #endregion
    }
}
