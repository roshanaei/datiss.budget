using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
using Datiss.Budget.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateIncomeCurrentOperationalViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ICOTypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int CountH { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int PriceH { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int CostH { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int CountNH { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int PriceNH { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int CostNH { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int TotalCount { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int TotalCost { get; set; }

        public IEnumerable<SelectListItem> ICOTypeSource { get; set; }

        public string ICOTypeTitle
        {
            get
            {
                if (ICOTypeSource == null || !ICOTypeSource.Any())
                    return string.Empty;
                return ICOTypeSource.FirstOrDefault(x => x.Value.ToString() == ICOTypeId.ToString()).Text;
            }
        }

        public IEnumerable<SelectListItem> ActivityTypeSource
            => EnumSelectListProvider.GetActivityTypeItems(ActivityType);
    }

    public class UpdateIncomeCurrentOperationalViewModel : CreateWWsFeeViewModel
    {
        public int Id { get; set; }
    }

    public class IncomeCurrentOperationalViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int ICOTypeId { get; set; }

        public string ICOTypeDisplay { get; set; }

        public int CountH { get; set; }

        public string CountHDisplay => CountH.ToString("N0");

        public int PriceH { get; set; }

        public string PriceHDisplay => PriceH.ToString("N0");

        public int CostH { get; set; }

        public string CostHDisplay => CostH.ToString("N0");

        public int CountNH { get; set; }

        public string CountNHDisplay => CountNH.ToString("N0");

        public int PriceNH { get; set; }

        public string PriceNHDisplay => PriceNH.ToString("N0");

        public int CostNH { get; set; }
        public string CostNHDisplay => CostNH.ToString("N0");

        public int TotalCount { get; set; }

        public string TotalCountDisplay => TotalCount.ToString("N0");

        public int TotalCost { get; set; }

        public string TotalCostDisplay => TotalCost.ToString("N0");
    }

    public class IncomeCurrentOperationalFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }

        public int? ICOTypeId { get; set; }

        public ActivityType? ActivityType { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> ActivityTypeSource => EnumSelectListProvider.GetActivityTypeItems(ActivityType).ToList();

    }

    public class IncomeCurrentOperationalIndexViewModel : PagedViewModel<IncomeCurrentOperationalViewModel>
    {
        public IncomeCurrentOperationalIndexViewModel()
        {
            Filter = new IncomeCurrentOperationalFilterViewModel();
        }

        public IncomeCurrentOperationalFilterViewModel Filter { get; set; }

        public ActivityType activityType { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> ICOTypeSource { get; set; }

        public string UserTypeSourceIdArray
        {
            get
            {
                if (ICOTypeSource == null || !ICOTypeSource.Any())
                    return string.Empty;
                string result = "";
                foreach (var item in ICOTypeSource)
                {
                    result += $"{item.Value},";
                }
                return result.TrimEnd(',');
            }
        }

        public IList<SelectListItem> UsageLayerSource { get; set; }

        public IList<SelectListItem> ActivityTypeSource { get; set; }

        public IFormFile ExcelFile { get; set; }

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

        public void SetICOTypeSource(IEnumerable<DropDownItemViewModel> source)
            => ICOTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetUsageLayerSource(IEnumerable<DropDownItemViewModel> source)
            => UsageLayerSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetActivityTypeSource(IEnumerable<SelectListItem> source)
            => ActivityTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value.ToString()
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
