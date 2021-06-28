using Datiss.Budget.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity;

namespace Datiss.Budget.Entities.Identity
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2577
    /// and http://www.dotnettips.info/post/2578
    /// </summary>
    public class RoleClaim : IdentityRoleClaim<int>, IAuditableEntity
    {
        public RoleClaim() { }
        
        #region Navigations

        public Role Role { get; set; }

        #endregion
    }
}