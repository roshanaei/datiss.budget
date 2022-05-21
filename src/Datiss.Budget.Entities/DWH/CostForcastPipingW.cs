using Datiss.Budget.Entities.AuditableEntity;


namespace Datiss.Budget.Entities.DWH
{
    public class CostForcastPipingW :IAuditableEntity
    {
        public CostForcastPipingW() { }

        #region Peroperties
        public  int Id { get; set; }    

        public int YearId { get; set; }

        public int TubeTypeId { get; set; }

        public int DiameterPipeTypeId { get; set; }

        public int DigTypeId { get; set; }

        public long TubeBuyCost { get; set; }

        public long RunCost { get; set; }   

        public long TotalCost { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Constant TubeType { get; set; }

        public Constant DiameterPipeType { get; set; }

        public Constant DigType { get; set; }

        #endregion

    }
}
