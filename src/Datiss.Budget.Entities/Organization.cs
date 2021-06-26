using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Entities.DWH;

namespace Datiss.Budget.Entities
{
    public class Organization : IAuditableEntity
    {
        public Organization()
        {
            Childrens = new HashSet<Organization>();
            WaterInstallFees = new HashSet<WaterInstallFee>();
        }

        public int Id { get; set; }

        public virtual Organization Parent { get; set; }
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsVillage { get; set; }


        public virtual ICollection<Organization> Childrens { get; set; }
        public virtual ICollection<WaterInstallFee> WaterInstallFees { get; set; }

    }
}