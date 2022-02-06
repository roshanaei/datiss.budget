using Datiss.Budget.Entities.AuditableEntity;
namespace Datiss.Budget.Entities.DWH
{
    public class IncomeForcast : IAuditableEntity
    {
        public IncomeForcast() { }

        #region Properties

        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int NumberUser { get; set; }

        public int UnitUser { get; set; }

        public long WaterInstllIncome { get; set; }

        public long WaterBranchIncome { get; set; }

        public long WaterNote2Income { get; set; }

        public long WaterNote3Income { get; set; }

        public long WNote11Income { get; set; }

        #endregion

        #region Navigation

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        #endregion

    }
}
