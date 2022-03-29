using Datiss.Budget.Entities.AuditableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;
namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentPMDep : IAuditableEntity
    {
        public CostCurrentPMDep() { }

        #region Properties

        public int Id { get; set; }
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public int CCPMDepTypeId { get; set; }
        public int CostCenterTypeId { get; set; }
        public long FinancePMCost { get; set; }
        public decimal RFinancePMCost_D { get; set; }
        public long FinanceDepCost { get; set; }
        public decimal RFinanceDepCost_D { get; set; }

        #endregion

        #region Navigation

        public FinanceYear FinanceYear { get; set; }
        public Organization Organization { get; set; }
        public Constant CCPMDepType { get; set; }
        public Constant CostCenterType { get; set; }
        
        #endregion
    }
}
