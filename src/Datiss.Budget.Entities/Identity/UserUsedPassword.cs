using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.Identity
{
    public class UserUsedPassword : IAuditableEntity
    {
        public UserUsedPassword() { }

        #region Properties

        public int Id { get; set; }

        public string HashedPassword { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        #endregion
    }
}