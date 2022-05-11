using Datiss.Budget.Entities.AuditableEntity;


namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentPrescriptionBaseInfo : IAuditableEntity
    {
        public CostCurrentPrescriptionBaseInfo() { }

        #region Properties

        public int Id { get; set; }

        public int YearId { get; set; }

        //حداقل دستمزد
        public long FixSalary { get; set; }

        //حق مسکن
        public long HouseRt { get; set; }

        //حق جذب
        public long EmployRight { get; set; }

        //فوق العاده منطقه
        public long RegionRight { get; set; }

        //بن کارگری
        public int Copun { get; set; }

        // حق اولاد
        public long ChildRt { get; set; }

        //حق خواروبار
        public long StuffRt { get; set; }

        // حق سختی کار
        public long HardWorkingRt { get; set; }

        //بهداشت و درمان
        public long Healths { get; set; }

        //مزد نیروی جدید
        public long NewFixSalary { get; set; }


        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        #endregion
    }
}
