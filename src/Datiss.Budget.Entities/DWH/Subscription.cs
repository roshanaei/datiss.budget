using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class Subscription : IAuditableEntity
    {
        public Subscription() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int SubW { get; set; }

        public int SubWs { get; set; }
        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        #endregion
    }
}
