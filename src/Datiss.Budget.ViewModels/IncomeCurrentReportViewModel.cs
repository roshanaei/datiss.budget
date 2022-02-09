using Datiss.Budget.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class UpdateIncomeCurrentReportViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int SectionTypeId { get; set; }

        public int UnitTypeId { get; set; }

        public ActivityType? Activity { get; set; }

        public long FunctionalYear_1 { get; set; }

        public long FunctionalBasicYear { get; set; }

        public long ApproveYear_1 { get; set; }

        public long ForcastY { get; set; }

    }

    public class IncomeCurrentReportViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int SectionTypeId { get; set; }
        public string SectionTypeDisplay { get; set; }
        public int UnitTypeId { get; set; }
        public string UnitTypeDisplay { get; set; }
        public ActivityType? Activity { get; set; }
        public string AvtivityName { get; set; }
        public long FunctionalYear_1 { get; set; }
        public string FunctionalYear_1Display => FunctionalYear_1.ToString("N0");
        public long FunctionalBasicYear { get; set; }
        public string FunctionalBasicYearDisplay => FunctionalBasicYear.ToString("N0");
        public long ApproveYear_1 { get; set; }
        public string ApproveYear_1Display => ApproveYear_1.ToString("N0");
        public long ForcastY { get; set; }
        public string ForcastYDisplay => ForcastY.ToString("N0");


    }

    public class IncomeCurrentReportFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class IncomeCurrentReportIndexViewModel : PagedViewModel<IncomeCurrentReportViewModel>
    {

        public IncomeCurrentReportIndexViewModel()
        {
            Filter = new IncomeCurrentReportFilterViewModel();
        }

        public IncomeCurrentReportFilterViewModel Filter { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public void SetYearSource(IEnumerable<DropDownItemViewModel> source)
            => YearSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => OrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetInputOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => InputOrganizationSource = source.Select(x => new SelectListItem
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
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }
}

