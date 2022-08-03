using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentPersonelDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public RecordType RecordType { get; set; }

        public int PersonelCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool GenderId { get; set; }

        public int GradeTypeId { get; set; }
        public string GradeTypeTitle { get; set; }

        public int ContractTypeId { get; set; }

        public int JobDepartmentTypeId { get; set; }

        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeTitle { get; set; }


        public int JobStatusTypeId { get; set; }

        public int JobStatusDetailTypeId { get; set; }

        public int ExperienceYear { get; set; }

        public int ExperienceMonth { get; set; }

        ////مزد ثابت سال جاری
        //public long FixSalary { get; set; }

        ////حق جذب سال جاری
        //public long EmployRight { get; set; }

        ////فوق العاده منطقه
        //public long RegionRight { get; set; }

        ////ساعت اضافه کاری
        //public int OverTimeValue { get; set; }

        //// مبلغ اضافه کاری
        //public long OverTimeCost { get; set; }

        ////تعداد روز  تعطیل کاری
        //public int HolidayValue { get; set; }

        //// مبلغ تعطیل کاری
        //public long HolidayCost { get; set; }

        ////درصد نوبت کاری
        //public long ShiftPercent { get; set; }
        ////نوبت کاری و کشیک
        //public long ShiftPCost { get; set; }

        //// تعداد ماموریت
        //public long MissionCount { get; set; }
        //// هزینه ماموریت
        //public long MissionDayCost { get; set; }

        //// حق سختی کار
        //public long HardWorkingRt { get; set; }

        //// حق ایاب و ذهاب
        //public long TrafficRt { get; set; }


        ////حق مسکن
        //public long HouseRt { get; set; }


        //// حق اولاد
        //public long ChildRt { get; set; }


        ////حق خواروبار
        //public long StuffRt { get; set; }


        ////آموزش
        //public long Education { get; set; }


        ////بیمه سهم کارفرما
        //public long InsuranceMaster { get; set; }

        //// بیمه عمر و حادثه تکمیلی
        //public long InsuranceAging { get; set; }


        ////عیدی سالیانه
        //public long HolidayYearly { get; set; }


        ////ذخیره مزایای پایان خدمت کارکنان
        //public long MilitaryServiceCost { get; set; }


        ////  سنوات
        //public long EndJobReward { get; set; }


        ////هزینه های رفاهی
        //public long WelfareCost { get; set; }

        ////ماه بازنشستگی
        //public int RetirementMonth { get; set; }
    }

    public class UpdateCostCurrentPersonelDTO : CreateCostCurrentPersonelDTO
    {
        public int Id { get; set; }
    }

    public class CostCurrentPersonelDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public RecordType RecordType { get; set; }

        public string RecordTypeDispaly { get; set; }

        public int PersonelCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool GenderId { get; set; }

        public int GradeTypeId { get; set; }
        public string GradeTypeDisplay { get; set; }

        public int ContractTypeId { get; set; }
        public string ContractTypeDisplay { get; set; }

        public int JobDepartmentTypeId { get; set; }
        public string JobDepartmentTypeDisplay { get; set; }

        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }

        public int JobStatusTypeId { get; set; }
        public string JobStatusTypeDisplay { get; set; }

        public int JobStatusDetailTypeId { get; set; }
        public string JobStatusDetailTypeDisplay { get; set; }

        public int ExperienceYear { get; set; }

        public int ExperienceMonth { get; set; }

        public long FixSalary { get; set; }

        public long EmployRight { get; set; }

        public long RegionRight { get; set; }

        public int OverTimeValue { get; set; }

        public long OverTimeCost { get; set; }

        public int HolidayValue { get; set; }

        public long HolidayCost { get; set; }

        public long ShiftPercent { get; set; }

        public long ShiftPCost { get; set; }

        public long MissionCount { get; set; }

        public long MissionDayCost { get; set; }

        public long HardWorkingRt { get; set; }

        public long TrafficRt { get; set; }

        public long HouseRt { get; set; }

        public long ChildRt { get; set; }

        public long StuffRt { get; set; }

        public long Education { get; set; }

        public long InsuranceMaster { get; set; }

        public long InsuranceAging { get; set; }

        public long HolidayYearly { get; set; }

        public long MilitaryServiceCost { get; set; }

        public long EndJobReward { get; set; }

        public long WelfareCost { get; set; }

        public int RetirementMonth { get; set; }
    }
}
