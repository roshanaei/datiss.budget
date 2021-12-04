using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class IncomeCurrentNOperational : IAuditableEntity
    {
        public IncomeCurrentNOperational () { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int NOICTypeId { get; set; }

        public int NOICPrice { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant NOICType { get; set; }
        #endregion
    }
}
