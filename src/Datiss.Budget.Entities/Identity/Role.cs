using System.Collections.Generic;
using Datiss.Budget.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity;

namespace Datiss.Budget.Entities.Identity
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2577
    /// and http://www.dotnettips.info/post/2578
    /// </summary>
    public class Role : IdentityRole<int>, IAuditableEntity
    {
        public Role() {}

        #region Properties

        public Role(string name)
            : this()
        {
            Name = name;
        }

        public Role(string name, string description)
            : this(name)
        {
            Description = description;
        }

        public string Title { get; set; }

        public string Description { get; set; }
        #endregion

        #region Navigations
        public  ICollection<UserRole> Users { get; set; }

        public  ICollection<RoleClaim> Claims { get; set; }
        #endregion
    }
}