using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class FeeCity : IAuditableEntity
    {
        public  FeeCity() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public decimal DomesticPrice { get; set; }

        public decimal NDomesticPrice { get; set; }

        #endregion

        #region Navigation
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }
        #endregion
    }
}
