using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class CostForcastFinance : IAuditableEntity
    {
        public CostForcastFinance() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int FinanceSubjectTypeId { get; set; }

        public long RemainingAssets { get; set; }

        public long AssetsCreated6_1 { get; set; }

        public long AssetsCreated6_2 { get; set; }

        public long ForcastAssets_D { get; set; }

        public long TotalAssetsCreated_D { get; set; }

        #endregion

        #region Navigation
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant CostCenter { get; set; }

        public Constant FinanceSubject { get; set; }


        #endregion
    }
}