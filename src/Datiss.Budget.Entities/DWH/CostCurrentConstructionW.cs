using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentConstructionW :IAuditableEntity
    {
        public CostCurrentConstructionW() { }

        #region Properties
        public int Id { get; set; } 

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public string ProjectDescription { get; set; }

        public int WaterInvestorsTypeId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int ExploitationAreaTypeId { get; set; }

        public int ProgressPercent { get;set; }

        public long CostDone { get; set; }

        public int Amount { get; set; } 

        public int MeasurementTypeId { get; set; }  

        public long UnitPrice { get;set;}   

        public long TotalCost { get; set; }     
        
        public int CreditTypeId { get; set; }   

        public int ExtensionTypeId { get; set; }

        public int SuggestedBudgetTopicTypeId { get;set ;}

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant CCCWWaterInvestors { get; set; }

        public Constant CCCWCostCenter { get; set; }

        public Constant CCCWExploitationArea { get; set; }

        public Constant CCCWMeasurement { get; set; }

        public Constant CCCWCredit { get; set; }

        public Constant CCCWExtension { get; set; }

        public Constant CCCWSuggestedBudgetTopic { get; set; }

        #endregion
    }
}
