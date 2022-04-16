using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentRawMaterialViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }
        public string ActivityTypeDisplay { get; set; }

        public int RawMaterialTypeId { get; set; }
        public int RawMaterialTypeDisplay { get; set; }

        public long BaseFee { get; set; }

        public long LastYearFee { get; set; }

    }

    public class UpdateCostCurrentRawMaterialViewModel : CreateCostCurrentRawMaterialViewModel
    {
        public int Id { get; set; }

        public long ForcastFee { get; set; }
    }

    public class CostCurrentRawMaterialViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int RawMaterialTypeId { get; set; }

        public string RawMaterialTypeDisplay { get; set; }

        public long BaseFee { get; set; }

        public string BaseFeeDisplay => BaseFee.ToString("N0");

        public long LastYearFee { get; set; }

        public string LastYearFeeDisplay => LastYearFee.ToString("N0");

        public long ForcastFee { get; set; }

        public string ForcastFeeDisplay => ForcastFee.ToString("N0");
    }

    public class CostCurrentRawMaterialFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }

        public int? RawMaterialTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IEnumerable<SelectListItem> ActivitySource => EnumSelectListProvider.GetActivityTypeItems();
    }

    public class CostCurrentRawMaterialIndexViewModel : PagedViewModel<CostCurrentRawMaterialViewModel>
    {
        public CostCurrentRawMaterialIndexViewModel()
        {
            Filter = new CostCurrentRawMaterialFilterViewModel();
        }

        public CostCurrentRawMaterialFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> RawMaterialSource { get; set; }

        public ActivityType ActivityType { get; set; }

        public IEnumerable<SelectListItem> ActivitySource => EnumSelectListProvider.GetActivityTypeItems(ActivityType);

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

        public void SetRawMaterialTypeSource(IEnumerable<DropDownItemViewModel> source)
            => RawMaterialSource = source.Select(x => new SelectListItem
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
