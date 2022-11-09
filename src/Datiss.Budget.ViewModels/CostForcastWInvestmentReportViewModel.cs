using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.ViewModels
{
    public class UpdateCostForcastWInvestmentReportViewModel :BaseViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public string YearDisplay { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CostCenterTypeId { get; set; }
        public int SectionTypeId { get; set; }
        public string SectionTypeDisplay { get; set; }
        public int UnitTypeId { get; set; }
        public string UnitTypeDisplay { get; set; }
        public int Amount1 { get; set; }
        public long Cost1 { get; set; }
        public int Amount2 { get; set; }
        public long Cost2 { get; set; }
        public int Amount3 { get; set; }
        public long Cost3 { get; set; }
        public int Amount4 { get; set; }
        public long Cost4 { get; set; }
    }


    public class CostForcastWInvestmentReportViewModel 
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public string YearDisplay { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CostCenterTypeId { get; set; }
        public int SectionTypeId { get; set; }
        public string SectionTypeDisplay { get; set; }
        public int UnitTypeId { get; set; }
        public string UnitTypeDisplay { get; set; }
        public int Amount1 { get; set; }
        public string Amount1Display => Amount1.ToString("N0");
        public long Cost1 { get; set; }
        public string Cost1Display => Cost1.ToString("N0");
        public int Amount2 { get; set; }
        public string Amount2Display => Amount2.ToString("N0");
        public long Cost2 { get; set; }
        public string Cost2Display => Cost2.ToString("N0");
        public int Amount3 { get; set; }
        public string Amount3Display => Amount3.ToString("N0");
        public long Cost3 { get; set; }
        public string Cost3Display => Cost3.ToString("N0");
        public int Amount4 { get; set; }
        public string Amount4Display => Amount4.ToString("N0");
        public long Cost4 { get; set; }
        public string Cost4Display => Cost4.ToString("N0");

    }


    public class CostForcastWInvestmentReportFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? NumberYear { get; set; }
        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
    }


    public class CostForcastWInvestmentReportIndexViewModel : PagedViewModel<CostCurrentProfitLossReportViewModel>
    {

        public CostForcastWInvestmentReportIndexViewModel()
        {
            Filter = new CostForcastWInvestmentReportFilterViewModel();
        }

        public CostForcastWInvestmentReportFilterViewModel Filter { get; set; }
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
