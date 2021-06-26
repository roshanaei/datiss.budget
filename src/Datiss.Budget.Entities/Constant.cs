using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Entities.DWH;

namespace Datiss.Budget.Entities
{
    public class Constant : IAuditableEntity
    {
        public Constant()
        {
            Childrens = new HashSet<Constant>();
            WaterInstallFees = new HashSet<WaterInstallFee>();
        }

        #region Properties

        public int Id { get; set; }

        public virtual Constant Parent { get; set; }
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string ConstantKey { get; set; }

        public int DisplayOrder { get; set; }

        #endregion

        #region Navigations

        public ICollection<Constant> Childrens { get; set; }
        public ICollection<WaterInstallFee> WaterInstallFees { get; set; }

        #endregion
    }
}