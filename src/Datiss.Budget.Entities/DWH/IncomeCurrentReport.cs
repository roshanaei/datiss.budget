using Datiss.Budget.Entities.AuditableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class IncomeCurrentReport : IAuditableEntity
    {
        public IncomeCurrentReport() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType? Activity { get; set; }

        public int SectionTypeId { get; set; }

        public int UnitTypeId { get; set; }

        public long FunctionalYear_1 { get; set; }

        public long FunctionalBasicYear { get; set; }

        public long ApproveYear_1 { get; set; }

        public long ForcastY { get; set; }

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant SectionType { get; set; }

        public Constant UnitType { get; set; }

        #endregion
    }
}
