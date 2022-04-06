using Datiss.Budget.Entities.AuditableEntity;


namespace Datiss.Budget.Entities.DWH
{
    public class  CostCurrentWaterSource :IAuditableEntity
    {
        public CostCurrentWaterSource() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; } 

        public int OrganizationId { get; set; }

        public int WaterSourceTypeId { get; set; }

        public int ActiveSource { get;set; }

        public long BaseProduction { get; set; }

        public long LastYearProduction { get;set;}

        public long ForcastProduction { get; set; }
        #endregion

        #region Navigations

        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }  

        public Constant WaterSourceType { get; set; }

        #endregion
    }
}
