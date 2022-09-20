using Datiss.Budget.Entities.AuditableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class IncomeForcastSourcesReport :IAuditableEntity
    {
        public IncomeForcastSourcesReport() { }

        #region Properties
        public int Id { get; set; }
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public int SourceDescriptionId { get;set; }
        public long FunctionalBasicYear { get; set; }
        public long FunctionalLastYear { get; set; }
        public long ApproveYear_1 { get; set; }
        public long PercentBudget { get; set; }
        public long ForcastY { get; set; }

        #endregion

        #region navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }  

        public Constant SourceDescription { get; set; }       
        #endregion
    }
}
