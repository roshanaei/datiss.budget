using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Entities.Identity;

namespace Datiss.Budget.Entities
{
    public class ReportRole : IAuditableEntity
    {
        public int Id { get; set; }

        public int ReportId { get; set; }

        public int RoleId { get; set; }

        #region Navigation

        public Report Report { get; set; }

        public Role Role { get; set; }

        #endregion
    }
}
