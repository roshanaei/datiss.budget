using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.DWH
{
    public class CostForcastBuy : IAuditableEntity
    {
        public CostForcastBuy() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public string BuyDescription { get; set; }

        public int LocationId { get; set; }

        public int BuyDepartmentId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int AssetTypeId { get; set; }

        public int AssetDetailTypeId { get; set; }

        public int Amount { get; set; }

        public int MeasurementTypeId { get; set; }

        public long UnitPrice { get; set; }

        public int CreditTypeId { get; set; }

        public long ProposedCost {get;set; }


        #endregion

        #region Navigation
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Organization Location { get; set; }

        public Constant Department { get; set; }

        public Constant CostCenter { get; set; }

        public Constant Asset { get; set; }

        public Constant AssetDetail { get; set; }

        public Constant Measurement { get; set; }

        public Constant Credit { get; set; }
            

        #endregion
    }
}
