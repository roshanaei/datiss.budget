using Datiss.Budget.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Datiss.Budget.Resources;
using Datiss.Budget.Extensions;

namespace Datiss.Budget.ViewModels
{


    public class CreateCostCurrentPersonelViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public RecordType RecordType { get; set; }
        public string RecordTypeDisplay => RecordType.ToDisplay();

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

        public int ExperienceMonth { get; set; }


        public IList<SelectListItem> InputOrganizationSource { get; set; }
        public IList<SelectListItem> GradeSource { get; set; }
        public IList<SelectListItem> JobDepartmentSource { get; set; }
        public IList<SelectListItem> JobStatusSource { get; set; }
        public IList<SelectListItem> JobStatusDetailSource { get; set; }
        public IList<SelectListItem> CostCenterSource { get; set; }
        public IList<SelectListItem> ContractSource { get; set; }

        public void SetInputOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => InputOrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetGradeSource(IEnumerable<DropDownItemViewModel> source)
            => GradeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetJobDepartment(IEnumerable<DropDownItemViewModel> source)
            => JobDepartmentSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetJobStatusSource(IEnumerable<DropDownItemViewModel> source)
            => JobStatusSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetJobStatusDetailSource(IEnumerable<DropDownItemViewModel> source)
            => JobStatusDetailSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetCostCenterSource(IEnumerable<DropDownItemViewModel> source)
            => CostCenterSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetContractSource(IEnumerable<DropDownItemViewModel> source)
            => ContractSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }
    public class UpdateCostCurrentPersonelViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

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


        public IList<SelectListItem> InputOrganizationSource { get; set; }
        public IList<SelectListItem> GradeSource { get; set; }
        public IList<SelectListItem> JobDepartmentSource { get; set; }
        public IList<SelectListItem> JobStatusSource { get; set; }
        public IList<SelectListItem> JobStatusDetailSource { get; set; }
        public IList<SelectListItem> CostCenterSource { get; set; }
        public IList<SelectListItem> ContractSource { get; set; }

        public void SetInputOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => InputOrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetGradeSource(IEnumerable<DropDownItemViewModel> source)
            => GradeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetJobDepartment(IEnumerable<DropDownItemViewModel> source)
            => JobDepartmentSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetJobStatusSource(IEnumerable<DropDownItemViewModel> source)
            => JobStatusSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetJobStatusDetailSource(IEnumerable<DropDownItemViewModel> source)
            => JobStatusDetailSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetCostCenterSource(IEnumerable<DropDownItemViewModel> source)
            => CostCenterSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetContractSource(IEnumerable<DropDownItemViewModel> source)
            => ContractSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
    }

    public class CostCurrentPersonelViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public RecordType RecordType { get; set; }
        public string RecordTypeDisplay => RecordType.ToDisplay(); 

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
        public string FixSalaryDisplay => FixSalary.ToString("N0");

        public long EmployRight { get; set; }
        public string EmployRightDisplay => EmployRight.ToString("N0");

        public long RegionRight { get; set; }
        public string RegionRightDisplay => RegionRight.ToString("N0");

        public int OverTimeValue { get; set; }
        public string OverTimeValueDisplay => OverTimeValue.ToString("N0");

        public long OverTimeCost { get; set; }
        public string OverTimeCostDisplay => OverTimeCost.ToString("N0");

        public int HolidayValue { get; set; }

        public long HolidayCost { get; set; }
        public string HolidayCostDisplay => HolidayCost.ToString("N0");

        public long ShiftPercent { get; set; }

        public long ShiftPCost { get; set; }

        public long MissionCount { get; set; }

        public long MissionDayCost { get; set; }
        public string MissionDayCostDisplay => MissionDayCost.ToString("N0");

        public long HardWorkingRt { get; set; }
        public string HardWorkingRtDisplay => HardWorkingRt.ToString("N0");

        public long TrafficRt { get; set; }
        public string TrafficRtDisplay => TrafficRt.ToString("N0");

        public long HouseRt { get; set; }
        public string HouseRtDisplay => HouseRt.ToString("N0");

        public long ChildRt { get; set; }
        public string ChildRtDisplay => ChildRt.ToString("N0");

        public long StuffRt { get; set; }
        public string StuffRtDisplay => StuffRt.ToString("N0");

        public long Education { get; set; }
        public string EducationDisplay => Education.ToString("N0");

        public long InsuranceMaster { get; set; }
        public string InsuranceMasterDisplay => InsuranceMaster.ToString("N0");

        public long InsuranceAging { get; set; }
        public string InsuranceAgingDisplay => InsuranceAging.ToString("N0");

        public long HolidayYearly { get; set; }
        public string HolidayYearlyDisplay => HolidayYearly.ToString("N0");

        public long MilitaryServiceCost { get; set; }
        public string MilitaryServiceCostDisplay => MilitaryServiceCost.ToString("N0");

        public long EndJobReward { get; set; }

        public long WelfareCost { get; set; }
        public string WelfareCostDisplay => WelfareCost.ToString("N0");



        public long RetirementMonth { get; set; }
        public string RetirementMonthDisplay => RetirementMonth.ToString("N0");
        


    }

    public class CostCurrentPersonelFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public RecordType? RecordType { get; set; }
        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> RecordTypeSource => EnumSelectListProvider.GetRecordTypeItems(RecordType).ToList();
    }

    public class CostCurrentPersonelIndexViewModel : PagedViewModel<CostCurrentPersonelViewModel>
    {

        public CostCurrentPersonelIndexViewModel()
        {
            Filter = new CostCurrentPersonelFilterViewModel();
        }

        public CostCurrentPersonelFilterViewModel Filter { get; set; }

        public RecordType recordType { get; set; }


        //public IList<SelectListItem> YearSource { get; set; }
        //public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> RecordTypeSource { get; set; }

        public IList<SelectListItem> JobStatusTypeSource { get; set; }

        public string JobStatusTypeSourceIdArray
        {
            get
            {
                if (JobStatusTypeSource == null || !JobStatusTypeSource.Any())
                    return string.Empty;
                string result = "";
                foreach (var item in JobStatusTypeSource)
                {
                    result += $"{item.Value},";
                }
                return result.TrimEnd(',');
            }
        }
        public void SetJobStatusTypeSource(IEnumerable<DropDownItemViewModel> source)
            => JobStatusTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrgId = null)
            => Filter.OrganizationSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
        {
            Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        }

    }

    public class CostCurrentPersonelImportViewModel : PagedViewModel<CostCurrentPersonelViewModel>
    {
        public IList<DropDownItemViewModel> GradeTypeSource { get; set; }
        public IList<DropDownItemViewModel> CostCenterTypeSource { get; set; }
        public IList<DropDownItemViewModel> ContractTypeSource { get; set; }
        public IList<DropDownItemViewModel> JobDepartmentTypeSource { get; set; }
        public IList<DropDownItemViewModel> JobStatusTypeSource { get; set; }
        public IList<DropDownItemViewModel> JobStatusDetailTypeSource { get; set; }
        public IList<DropDownItemViewModel> OrganizationSource { get; set; }

    }
}

