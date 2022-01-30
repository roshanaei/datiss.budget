using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public Role() {
            Users = new HashSet<UserRole>();
            Claims = new HashSet<RoleClaim>();
        }
        

        #region Properties

        public Role(string name) : this()
        {
            Name = name;
            Claims = new HashSet<RoleClaim>();
            Users = new HashSet<UserRole>();
        }

        public Role(string name, string description) : this(name)
        {
            Description = description;
            Claims = new HashSet<RoleClaim>();
            Users = new HashSet<UserRole>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public bool IsConstantRole => Name.ToUpper() == "ADMIN";

        #endregion

        #region Navigations
        public  ICollection<UserRole> Users { get; set; }

        public  ICollection<RoleClaim> Claims { get; set; }
        #endregion
    }
}