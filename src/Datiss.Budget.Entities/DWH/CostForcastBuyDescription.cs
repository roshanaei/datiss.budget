using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH

{
    public class CostForcastBuyDescription : IAuditableEntity
    {
        public CostForcastBuyDescription() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int AssetTypeId { get; set; }

        public int AssetDetailTypeId { get; set; }

        public int MeasurementTypeId { get; set; }

        public long UnitPrice { get; set; }


        #endregion

        #region Navigation
        public FinanceYear FinanceYear { get; set; }

        public Constant Asset { get; set; }

        public Constant AssetDetail { get; set; }

        public Constant Measurement { get; set; }


        #endregion
    }
}
