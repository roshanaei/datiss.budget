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
    public class CreateWWsFeeViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int UserTypeId { get; set; }

        public int UsageLayerId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int P1Fee { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int P2Fee { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int P1Note3 { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int P2Note3 { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int P1Note7 { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public int P2Note7 { get; set; }

        public IEnumerable<SelectListItem> UserTypeSource { get; set; }

        public string UserTypeTitle
        {
            get
            {
                if (UserTypeSource == null || !UserTypeSource.Any())
                    return string.Empty;
                return UserTypeSource.FirstOrDefault(x => x.Value.ToString() == UserTypeId.ToString()).Text;
            }
        }

        public IEnumerable<SelectListItem> ActivityTypeSource
            => EnumSelectListProvider.GetActivityTypeItems(ActivityType);
    }

    public class UpdateWWsFeeViewModel : CreateWWsFeeViewModel
    {
        public int Id { get; set; }
    }

    public class WWsFeeViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public int UsageLayerId { get; set; }

        public string UsageLayerDisplay { get; set; }

        public int P1Fee { get; set; }

        public string P1FeeDisplay => P1Fee.ToString("N0");

        public int P2Fee { get; set; }

        public string P2FeeDisplay => P2Fee.ToString("N0");

        public int P1Note3 { get; set; }

        public string P1Note3Display => P1Note3.ToString("N0");

        public int P2Note3 { get; set; }

        public string P2Note3Display => P2Note3.ToString("N0");

        public int P1Note7 { get; set; }

        public string P1Note7Display => P1Note7.ToString("N0");

        public int P2Note7 { get; set; }

        public string P2Note7Display => P2Note7.ToString("N0");
    }

    public class WWsFeeFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }

        public int? UserTypeId { get; set; }

        public int? UsageLayerId { get; set; }

        public ActivityType? ActivityType { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> ActivityTypeSource => EnumSelectListProvider.GetActivityTypeItems(ActivityType).ToList();

    }

    public class WWsFeeIndexViewModel : PagedViewModel<WWsFeeViewModel>
    {
        public WWsFeeIndexViewModel()
        {
            Filter = new WWsFeeFilterViewModel();
        }

        public WWsFeeFilterViewModel Filter { get; set; }

        public ActivityType activityType { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> UserTypeSource { get; set; }

        public string UserTypeSourceIdArray
        {
            get
            {
                if (UserTypeSource == null || !UserTypeSource.Any())
                    return string.Empty;
                string result = "";
                foreach (var item in UserTypeSource)
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

        public void SetUserTypeSource(IEnumerable<DropDownItemViewModel> source)
            => UserTypeSource = source.Select(x => new SelectListItem
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
