using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public  class CostCurrentReport : IAuditableEntity
    {
        public CostCurrentReport() { }

        #region Properties

        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int SectionTypeId { get; set; }

        public int UnitTypeId { get; set; }

        public int UnitDetailTypeId { get; set; }

        public int CostCenterTypeId { get; set; }

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

        public Constant UnitDetailType { get; set; }

        public Constant CostCenterType { get; set; }

        #endregion
    }
}
