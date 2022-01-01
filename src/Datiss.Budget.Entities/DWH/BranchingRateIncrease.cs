using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class BranchingRateIncrease : IAuditableEntity
    {
        public BranchingRateIncrease() { }

        #region Properties

        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int WaterRateIncrease { get; set; }

        public int WasteRateIncrease { get; set; }

        public int WastePersentIncrease { get; set; }

        public int FixAmountBusiness { get; set; }

        public int CapacityFixAmount { get; set; }

        public int WaterInstallRateIncrease { get; set; }

        public int WsInstalIncrease { get; set; }

        public int WaterFixNote2 { get; set; }

        public int WasteFixNote2 { get; set; }

        #endregion

        #region navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        #endregion
    }
}
