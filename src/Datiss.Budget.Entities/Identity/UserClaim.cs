using Datiss.Budget.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity;

namespace Datiss.Budget.Entities.Identity
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2577
    /// and http://www.dotnettips.info/post/2578
    /// </summary>
    public class UserClaim : IdentityUserClaim<int>, IAuditableEntity
    {
        public UserClaim() { }

        #region Properties
        public User User { get; set; }

        #endregion
    }
}