using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Entities.Identity;

namespace Datiss.Budget.Entities
{
    public class Organization : IAuditableEntity
    {
        public Organization()
        {
            Childrens = new HashSet<Organization>();
            WaterInstallFees = new HashSet<WaterInstallFee>();
        }

        #region Properties

        public int Id { get; set; }

        
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsVillage { get; set; }

        #endregion

        #region Navigations

        public Organization Parent { get; set; }

        public ICollection<Organization> Childrens { get; set; }
        
        public ICollection<WaterInstallFee> WaterInstallFees { get; set; }
        
        public ICollection<User> Users { get; set; }

        #endregion
    }
}