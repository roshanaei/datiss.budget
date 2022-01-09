using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateIncomeForcastOtherViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int OIFTypeId { get; set; }

        public string OIFTypeTitle
        {
            get
            {
                if (OIFTypeSource == null || !OIFTypeSource.Any())
                    return string.Empty;

                return OIFTypeSource.FirstOrDefault(x => x.Value.ToString() == OIFTypeId.ToString()).Text;
            }
        }

        public ActivityType ActivityId { get; set; }

        public int OIFCount { get; set; }

        public int OIFPrice { get; set; }

        public IEnumerable<SelectListItem> OIFTypeSource { get; set; }

        public IEnumerable<SelectListItem> ActivitySource => EnumSelectListProvider.GetActivityTypeItems(ActivityId);

    }

    public class UpdateIncomeForcastOtherViewModel : CreateIncomeForcastOtherViewModel
    {
        public int Id { get; set; }
    }

    public class IncomeForcastOtherViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int OIFTypeId { get; set; }
        public string OIFTypeDisplay { get; set; }
        public ActivityType ActivityId { get; set; }
        public string ActivityDisplay => ActivityId.ToDisplay();
        public int OIFCount { get; set; }
        public string OIFCountDisplay => OIFCount.ToString("N0");
        public int OIFPrice { get; set; }
        public string OIFPriceDisplay => OIFPrice.ToString("N0");
    }

    public class IncomeForcastOtherFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? OIFTypeId { get; set; }
        public ActivityType? ActivityId { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IEnumerable<SelectListItem> ActivitySource => EnumSelectListProvider.GetActivityTypeItems(ActivityId);
    }

    public class IncomeForcastOtherIndexViewModel : PagedViewModel<IncomeForcastOtherViewModel>
    {

        public IncomeForcastOtherIndexViewModel()
        {
            Filter = new IncomeForcastOtherFilterViewModel();
        }

        public IncomeForcastOtherFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> OIFTypeSource { get; set; }

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

        public void SetOIFTypeSource(IEnumerable<DropDownItemViewModel> source)
            => OIFTypeSource = source.Select(x => new SelectListItem
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
