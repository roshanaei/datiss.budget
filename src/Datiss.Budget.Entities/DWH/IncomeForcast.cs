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

        public int WaterInstllIncome { get; set; }

        public int WaterBranchIncome { get; set; }

        public int WaterNote2Income { get; set; }

        public int WaterNote3Income { get; set; }

        public int WNote11Income { get; set; }

        #endregion

        #region Navigation

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        #endregion

    }
}
