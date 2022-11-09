using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class CostForcastWInvestmentReport :IAuditableEntity
    {
        public CostForcastWInvestmentReport() { }
        #region Properties
        public int Id { get; set; }
        public int YearId { get; set; } 
        public int OrganizationId { get; set; }
        public int CostCenterTypeId { get; set; }
        public int SectionTypeId { get; set; }  
        public int UnitTypeId { get; set; }
        public int Amount1 { get; set; }
        public long Cost1 { get; set; } 
        public int Amount2 { get; set; }
        public long Cost2 { get; set; }
        public int Amount3 { get; set; }
        public long Cost3 { get; set; }
        public int Amount4 { get; set; }
        public long Cost4 { get; set; }

        #endregion

        #region Navigation
        public FinanceYear FinanceYear { get; set; }
        public Organization Organization { get; set; }
        public Constant CostCenterType { get; set; }
        public Constant SectionType { get; set; }
        public Constant UnitType { get; set; }  
        #endregion
    }
}
