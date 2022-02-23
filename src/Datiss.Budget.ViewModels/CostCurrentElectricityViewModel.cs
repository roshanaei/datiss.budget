using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentElectricityViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ElectricityAmount { get; set; }

        public long ElectricityCost { get; set; }

    }

    public class UpdateCostCurrentElectricityViewModel : CreateCostCurrentElectricityViewModel
    {
        public int Id { get; set; }
    }
    public class CostCurrentElectricityViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int ElectricityAmount { get; set; }

        public string ElectricityAmountDisplay => ElectricityAmount.ToString("N0");

        public long ElectricityCost { get; set; }

        public string ElectricityCostDisplay => ElectricityCost.ToString("N0");
    }

    public class CostCurrentElectricityFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? ActivityType { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> ActivityTypeSource => EnumSelectListProvider.GetActivityTypeItems(ActivityType).ToList();
    }

    public class CostCurrentElectricityIndexViewModel : PagedViewModel<CostCurrentElectricityViewModel>
    {
        public CostCurrentElectricityIndexViewModel()
        {
            Filter = new CostCurrentElectricityFilterViewModel();
        }
        public CostCurrentElectricityFilterViewModel Filter { get; set; }

        public ActivityType activityType { get; set; }

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
