using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{

    public class CreateCostCurrentPrescriptionBaseInfoViewModel: BaseViewModel
    {

        public int YearId { get; set; }
        public string YearDisplay { get; set; }
        public long FixSalary { get; set; }
        public long HouseRt { get; set; }
        public long EmployRight { get; set; }
        public long RegionRight { get; set; }
        public int Copun { get; set; }
        public long ChildRt { get; set; }
        public long StuffRt { get; set; }
        public long HardWorkingRt { get; set; }
        public long Healths { get; set; }
        public long NewFixSalary { get; set; }

    }

    public class UpdateCostCurrentPrescriptionBaseInfoViewModel : CreateCostCurrentPrescriptionBaseInfoViewModel
    {
        public int Id { get; set; }

    }

    public class CostCurrentPrescriptionBaseInfoViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public long FixSalary { get; set; }
        public string FixSalaryDisplay => FixSalary.ToString("N0");
        public long HouseRt { get; set; }
        public string HouseRtDisplay => HouseRt.ToString("N0");
        public long EmployRight { get; set; }
        public string EmployRightDisplay => EmployRight.ToString("N0");
        public long RegionRight { get; set; }
        public string RegionRightDisplay => RegionRight.ToString("N0");
        public int Copun { get; set; }
        public string CopunDisplay => Copun.ToString("N0");
        public long ChildRt { get; set; }
        public string ChildRtDisplay => ChildRt.ToString("N0");
        public long StuffRt { get; set; }
        public string StuffRtDisplay => StuffRt.ToString("N0");
        public long HardWorkingRt { get; set; }
        public string HardWorkingRtDisplay => HardWorkingRt.ToString("N0");
        public long Healths { get; set; }
        public string HealthsDisplay => Healths.ToString("N0");
        public long NewFixSalary { get; set; }
        public string NewFixSalaryDisplay => NewFixSalary.ToString("N0");
    }

    public class CostCurrentPrescriptionBaseInfoFilterViewModel: FilterViewModel
    {
        public int? YearId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
    }

    public class CostCurrentPrescriptionBaseInfoIndexViewModel : PagedViewModel<CostCurrentPrescriptionBaseInfoViewModel> 
    {

        public CostCurrentPrescriptionBaseInfoIndexViewModel() {
            Filter = new CostCurrentPrescriptionBaseInfoFilterViewModel();
        }

        public CostCurrentPrescriptionBaseInfoFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public void SetYearSource(IEnumerable<DropDownItemViewModel> source) 
            => YearSource = source.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();      

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null) 
            => Filter.YearSource = source.Select(x => new SelectListItem {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        
    }

}
