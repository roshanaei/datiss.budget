using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Entities.DWH
{
    public class Cofficient : IAuditableEntity
    {
        public Cofficient()
        {

        }
        #region Properties
        public int Id { get; set; }
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public CofficientsGroup GroupName { get; set; }
        public EntityStatus Status { get; set; }
        public int CofficientTypeId { get; set; }
        public decimal Fee { get; set; }
        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }
        public Organization Organization { get; set; }
        public Constant CofficientType { get; set; }
        #endregion
    }
}
