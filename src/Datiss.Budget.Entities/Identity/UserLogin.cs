using Datiss.Budget.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity;

namespace Datiss.Budget.Entities.Identity
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2577
    /// and http://www.dotnettips.info/post/2578
    /// </summary>
    public class UserLogin : IdentityUserLogin<int>, IAuditableEntity
    {
        public UserLogin() { }

        #region Properties

        public string IpAddress { get; set; }

        public string UserAgent { get; set; }

        public string HostName { get; set; }

        #endregion

        public virtual User User { get; set; }
    }
}