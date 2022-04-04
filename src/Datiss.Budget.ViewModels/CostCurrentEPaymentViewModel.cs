using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentEPaymentViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int BillingCycle { get; set; }

        public decimal EPayForcast { get; set; }

        public long EPayBFee { get; set; }

        public decimal PPayForcast { get; set; }

        public long PPayBFee { get; set; }

    }

    public class UpdateCostCurrentEPaymentViewModel : CreateCostCurrentEPaymentViewModel
    {
        public int Id { get; set; }

    }

    public class CostCurrentEPaymentViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int BillingCycle { get; set; }
        public string BillingCycleDisplay => BillingCycle.ToString("N0");

        public decimal EPayForcast { get; set; }
        public string EPayForcastDisplay => EPayForcast.ToString("N2");

        public long EPayBFee { get; set; }
        public string EPayBFeeDisplay => EPayBFee.ToString("N0");

        public decimal PPayForcast { get; set; }
        public string PPayForcastDisplay => PPayForcast.ToString("N2");

        public long PPayBFee { get; set; }
        public string PPayBFeeDisplay => PPayBFee.ToString("N0");
    }

    public class CostCurrentEPaymentFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostCurrentEPaymentIndexViewModel : PagedViewModel<CostCurrentEPaymentViewModel>
    {

        public CostCurrentEPaymentIndexViewModel()
        {
            Filter = new CostCurrentEPaymentFilterViewModel();
        }

        public CostCurrentEPaymentFilterViewModel Filter { get; set; }

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
