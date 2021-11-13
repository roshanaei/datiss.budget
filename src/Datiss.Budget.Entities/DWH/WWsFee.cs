using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class WWsFee : IAuditableEntity
    {
        public WWsFee() { }

        #region Properties

        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int UserTypeId { get; set; }

        public int UsageLayerId { get; set; }

        public int P1Fee { get; set; }

        public int P2Fee { get; set; }

        public int P1Note3 { get; set; }

        public int P2Note3 { get; set; }

        public int P1Note7 { get; set; }

        public int P2Note7 { get; set; }

        #endregion

        #region Navigation

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }
        #endregion
    }
}
