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
    public class CreateCostCurrentInstallationViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public int CCInstalationTypeId { get; set; }

        public string CCInstalationTypeTitle
        {
            get
            {
                if (CCInstalationTypeSource == null || !CCInstalationTypeSource.Any())
                    return string.Empty;

                return CCInstalationTypeSource.FirstOrDefault(x => x.Value.ToString() == CCInstalationTypeId.ToString()).Text;
            }
        }

        public int NumberUser { get; set; }

        public int Cost { get; set; }

        public long Income { get; set; }

        public IEnumerable<SelectListItem> CCInstalationTypeSource { get; set; }
    }

    public class UpdateCostCurrentInstallationViewModel : CreateCostCurrentInstallationViewModel
    {
        public int Id { get; set; }
    }

    public class CostCurrentInstallationViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int CCInstalationTypeId { get; set; }

        public string CCInstalationTypeDisplay { get; set; }

        public int NumberUser { get; set; }

        public string NumberUserDisplay => NumberUser.ToString("N0");

        public int Cost { get; set; }

        public string CostDisplay => Cost.ToString("N0");

        public long Income { get; set; }

        public string IncomeDisplay => Income.ToString("N0");
    }

    public class CostCurrentInstallationFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
    
        public int? OrganizationId { get; set; }
        
        public int? CCInstalationTypeId { get; set; }
        
        public IList<SelectListItem> YearSource { get; set; }
        
        public IList<SelectListItem> OrganizationSource { get; set; }
        
        public IEnumerable<SelectListItem> ActivitySource => EnumSelectListProvider.GetActivityTypeItems();
    }

    public class CostCurrentInstallationIndexViewModel : PagedViewModel<CostCurrentInstallationViewModel>
    {
        public CostCurrentInstallationIndexViewModel()
        {
            Filter = new CostCurrentInstallationFilterViewModel();
        }

        public CostCurrentInstallationFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        //public IList<SelectListItem> CCInstalationTypeSource { get; set; }

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

        //public void SetCCInstalationTypeSource(IEnumerable<DropDownItemViewModel> source)
        //    => CCInstalationTypeSource = source.Select(x => new SelectListItem
        //    {
        //        Text = x.Title,
        //        Value = x.Id.ToString()
        //    }).ToList();

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
