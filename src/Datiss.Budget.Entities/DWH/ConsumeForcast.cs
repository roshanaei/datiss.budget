using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class ConsumeForcast : IAuditableEntity
    {
        public  ConsumeForcast() {}

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }
        
        public int UserTypeId { get; set; }

        public int UsageLayerId { get; set; }

        public decimal CountUser { get; set; }

        public decimal UnitUser { get; set; }

        public decimal ConsumeUser { get; set; }

        public decimal AvgConsumeUser { get; set; }

        public decimal ConsumeUserForcast { get; set; }

        #endregion

        #region Navigation
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        public Constant UsageLayer { get; set; }
        #endregion

    }
}
