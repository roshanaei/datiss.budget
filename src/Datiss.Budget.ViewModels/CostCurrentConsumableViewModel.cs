using Datiss.Budget.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Datiss.Budget.Extensions;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentConsumableViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public ActivityType ActivityType { get; set; }
        public string ConsumableTypeDisplay { get; set; }
        public int ConsumableTypeId { get; set; }
        public int ConsumableAmount { get; set; }
        public long ConsumableCost { get; set; }
        public IEnumerable<SelectListItem> ActivityTypeSource => EnumSelectListProvider.GetActivityTypeItems(ActivityType);
    }

    public class UpdateCostCurrentConsumableViewModel : CreateCostCurrentConsumableViewModel
    {
        public int Id { get; set; }
    }

    public class CostCurrentConsumableViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public ActivityType ActivityType { get; set; }
        public string ActivityTypeDisplay => ActivityType.ToDisplay();
        public int ConsumableTypeId { get; set; }
        public string ConsumableTypeDisplay { get; set; }
        public int ConsumableAmount { get; set; }
        public string ConsumableAmountDisplay => ConsumableAmount.ToString("N0");
        public long ConsumableCost { get; set; }
        public string ConsumableCostDisplay => ConsumableCost.ToString("N0");
    }

    public class CostCurrentConsumableFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? ActivityType { get; set; }
        public int? ConsumableTypeId { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> ActivityTypeSource => EnumSelectListProvider.GetActivityTypeItems(ActivityType).ToList();
    }

    public class CostCurrentConsumableIndexViewModel : PagedViewModel<CostCurrentConsumableViewModel>
    {

        public CostCurrentConsumableIndexViewModel()
        {
            Filter = new CostCurrentConsumableFilterViewModel();
        }

        public CostCurrentConsumableFilterViewModel Filter { get; set; }

        public ActivityType ActivityType { get; set; }

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
