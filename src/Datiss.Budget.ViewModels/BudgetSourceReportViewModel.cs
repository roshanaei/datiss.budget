using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.ViewModels
{
    public class UpdateBudgetSourceReportViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int SectionTypeId { get; set; }

        public long FunctionalYear_1 { get; set; }

        public long FunctionalBasicYear { get; set; }

        public long ApproveYear_1 { get; set; }

        public long ForcastY { get; set; }

        public decimal ReceiptPercent { get; set; }

        public long Fee { get; set; }

        public decimal ForcastFunctionalPercent { get; set; }

        public decimal ForcastBudgetPercent { get; set; }

    }

    public class BudgetSourceReportViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int SectionTypeId { get; set; }
        public string SectionTypeDisplay { get; set; }
        public long FunctionalYear_1 { get; set; }
        public string FunctionalYear_1Display => FunctionalYear_1.ToString("N0");
        public long FunctionalBasicYear { get; set; }
        public string FunctionalBasicYearDisplay => FunctionalBasicYear.ToString("N0");
        public long ApproveYear_1 { get; set; }
        public string ApproveYear_1Display => ApproveYear_1.ToString("N0");
        public long ForcastY { get; set; }
        public string ForcastYDisplay => ForcastY.ToString("N0");
        public decimal ReceiptPercent { get; set; }
        public string ReceiptPercentDisplay => ReceiptPercent.ToString("N2");
        public long Fee { get; set; }
        public string FeeDisplay => Fee.ToString("N0");
        public decimal ForcastFunctionalPercent { get; set; }
        public string ForcastFunctionalPercentDisplay => ForcastFunctionalPercent.ToString("N2");
        public decimal ForcastBudgetPercent { get; set; }
        public string ForcastBudgetPercentDisplay => ForcastBudgetPercent.ToString("N2");


    }

    public class BudgetSourceReportFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? NumberYear { get; set; }
        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class BudgetSourceReportIndexViewModel : PagedViewModel<BudgetSourceReportViewModel>
    {

        public BudgetSourceReportIndexViewModel()
        {
            Filter = new BudgetSourceReportFilterViewModel();
        }

        public BudgetSourceReportFilterViewModel Filter { get; set; }
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
        {
            Filter.YearSource = source.Select(x => new SelectListItem
               {
                   Selected = x.Id == selectedYearId,
                   Text = x.Title,
                   Value = x.Id.ToString()
               }).ToList();
        }

    }
}

