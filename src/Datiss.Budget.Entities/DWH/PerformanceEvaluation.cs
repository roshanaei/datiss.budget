using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
   public class PerformanceEvaluation: IAuditableEntity
    {
        public PerformanceEvaluation() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public EntityStatus Status { get; set; }

        public int TableFieldId { get; set; }

        public decimal Target { get; set; }

        public decimal Operation { get; set; }

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public TablesFiledTitle TablesFiled { get; set; }

        #endregion
    }
}
