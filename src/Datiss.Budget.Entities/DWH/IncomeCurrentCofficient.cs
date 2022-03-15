using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class IncomeCurrentCofficient : IAuditableEntity
    {
        public IncomeCurrentCofficient() {}

        #region Properties

        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }
        
        public int UserTypeId { get; set; }

        public int UsageLayerId { get; set; }

        public decimal Fee { get; set; }


        #endregion

        #region Navigation
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        public Constant UsageLayer { get; set; }
        #endregion

    }
}
