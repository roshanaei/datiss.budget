using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class WaterInstallFee : IAuditableEntity
    {
        public int Id { get; set; }

        public virtual FinanceYear FinanceYear { get; set; }
        public int  YearId { get; set; }

        public virtual Organization Organization { get; set; }
        public int OrganizationId { get; set; }

        public virtual Constant DWaterType { get; set; }
        public int DWaterTypeId { get; set; }

        public float WInstllFee { get; set; }

     }
}