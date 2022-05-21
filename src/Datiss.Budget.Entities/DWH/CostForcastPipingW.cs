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

        public int DiamaterPipeTypeId { get; set; }

        public int DigTypeId { get; set; }

        public long TubeBuyCost { get; set; }

        public long RunCost { get; set; }   

        public long TotalCost { get; set; }

        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Constant TubeType { get; set; }

        public Constant DiamaterPipeType { get; set; }

        public Constant DigType { get; set; }

        #endregion

    }
}
