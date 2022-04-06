using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentSharingSetadViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int WUnit { get; set; }

        public long IncomeCurrentW { get; set; }

        public decimal IncomeCurrentWSharingCoff { get; set; }

        public int WsUnit { get; set; }

        public long IncomeCurrentWs { get; set; }

        public decimal IncomeCurrentWsSharingCoff { get; set; }

        public long IncomeForcast { get; set; }

        public decimal SPSHahrdari { get; set; }

        public decimal IncomeForcastsharing { get; set; }

    }

    public class UpdateCostCurrentSharingSetadViewModel : CreateCostCurrentSharingSetadViewModel
    {
        public int Id { get; set; }

    }

    public class CostCurrentSharingSetadViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int WUnit { get; set; }
        public string WUnitDisplay => WUnit.ToString("N0");

        public long IncomeCurrentW { get; set; }
        public string IncomeCurrentWDisplay => IncomeCurrentW.ToString("N0");

        public decimal IncomeCurrentWSharingCoff { get; set; }
        public string IncomeCurrentWSharingCoffDisplay => IncomeCurrentWSharingCoff.ToString("N2");

        public int WsUnit { get; set; }
        public string WsUnitDisplay => WsUnit.ToString("N0");

        public long IncomeCurrentWs { get; set; }
        public string IncomeCurrentWsDisplay => IncomeCurrentWs.ToString("N0");

        public decimal IncomeCurrentWsSharingCoff { get; set; }
        public string IncomeCurrentWsSharingCoffDisplay => IncomeCurrentWsSharingCoff.ToString("N2");

        public long IncomeForcast { get; set; }
        public string IncomeForcastDisplay => IncomeForcast.ToString("N0");

        public decimal SPSHahrdari { get; set; }
        public string SPSHahrdariDisplay => SPSHahrdari.ToString("N2");

        public decimal IncomeForcastsharing { get; set; }
        public string IncomeForcastsharingDisplay => IncomeForcastsharing.ToString("N2");

    }

    public class CostCurrentSharingSetadFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostCurrentSharingSetadIndexViewModel : PagedViewModel<CostCurrentSharingSetadViewModel>
    {

        public CostCurrentSharingSetadIndexViewModel()
        {
            Filter = new CostCurrentSharingSetadFilterViewModel();
        }

        public CostCurrentSharingSetadFilterViewModel Filter { get; set; }

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
