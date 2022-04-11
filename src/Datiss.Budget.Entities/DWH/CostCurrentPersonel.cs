using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;


namespace Datiss.Budget.Entities.DWH
{
    public class CostCurrentPersonel : IAuditableEntity
    {
        public CostCurrentPersonel() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public RecordType RecordType { get; set; }

        public int PersonelCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool GenderId { get; set; }

        public int GradeTypeId { get; set; }

        public int ContractTypeId { get; set; }

        public int JobDepartmentTypeId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int JobStatusTypeId { get; set; }

        public int JobStatusDetailTypeId { get; set; }

        public int ExperienceYear { get; set; }

        public int ExperienceMonth { get; set; }

        //مزد ثابت سال جاری
        public long FixSalary { get; set; }

        //حق جذب سال جاری
        public long EmployRight { get; set; }

        //فوق العاده منطقه
        public long RegionRight { get; set; }

        //ساعت اضافه کاری
        public int OverTimeValue { get; set; }

        // مبلغ اضافه کاری
        public long OverTimeCost { get; set; }

        //تعداد روز  تعطیل کاری
        public int HolidayValue { get; set; }

        // مبلغ تعطیل کاری
        public long HolidayCost { get; set; }

        //درصد نوبت کاری
        public long ShiftPercent { get; set; }
        //نوبت کاری و کشیک
        public long ShiftPCost { get; set; }

        // تعداد ماموریت
        public long MissionCount { get; set; }
        // هزینه ماموریت
        public long MissionDayCost { get; set; }

        // حق سختی کار
        public long HardWorkingRt { get; set; }    

        // حق ایاب و ذهاب
        public long TrafficRt  { get; set; }


        //حق مسکن
        public long HouseRt { get; set; }


        // حق اولاد
        public long ChildRt { get; set; }


        //حق خواروبار
        public long StuffRt { get; set; }


        //آموزش
        public long Education { get; set; }


        //بیمه سهم کارفرما
        public long InsuranceMaster { get; set; }

        // بیمه عمر و حادثه تکمیلی
        public long InsuranceAging { get; set; }


        //عیدی سالیانه
        public long HolidayYearly { get; set; }


        //ذخیره مزایای پایان خدمت کارکنان
        public long MilitaryServiceCost { get; set; }


        //   مرخصی استفاده نشده
        public long UnUseHolidayCount { get; set; }


        //هزینه های رفاهی
        public long WelfareCost { get; set; }


        #endregion

        #region Navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant CostCenter { get; set; }

        public Constant Grade { get; set; }

        public Constant Contract { get; set; }

        public Constant JobDepartment { get; set; }

        public Constant JobStatus { get; set; }

        public Constant JobStatusDetail { get; set; }

        #endregion
    }
}
