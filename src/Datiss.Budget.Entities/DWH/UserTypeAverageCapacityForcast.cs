using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class UserTypeAverageCapacityForcast : IAuditableEntity
    {
        public UserTypeAverageCapacityForcast() { }

        #region Properties

        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public decimal AverageCapacityW { get; set; }

        public decimal AverageCapacityWs { get; set; }

        public decimal AverageCapacityWIncome { get; set; }

        public decimal AverageCapacityWsIncome { get; set; }

        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        #endregion
    }
}