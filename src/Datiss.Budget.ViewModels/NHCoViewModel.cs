using Datiss.Budget.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Datiss.Budget.Extensions;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateNHCoViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "لطفاً مبلغ را بصورت صحیح وارد نمایید.")]
        public int WInstallFee { get; set; }
        public ActivityType ActivityType { get; set; }
        public int P1Capacity { get; set; }
        public int FixCostCo { get; set; }
        public int P1CostCo { get; set; }
        public int P2CostCo { get; set; }
        public IEnumerable<SelectListItem> ActivityTypeSource => EnumSelectListProvider.GetActivityTypeItems(ActivityType);
    }

    public class UpdateNHCoViewModel : CreateNHCoViewModel
    {
        public int Id { get; set; }
    }

    public class NHCoViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public ActivityType ActivityType { get; set; }
        public string ActivityTypeDisplay => ActivityType.ToDisplay();
        public int P1Capacity { get; set; }
        public string P1CapacityDisplay => P1Capacity.ToString("N0");
        public int FixCostCo { get; set; }
        public string FixCostCoDisplay => FixCostCo.ToString("N0");
        public int P1CostCo { get; set; }
        public string P1CostCoDisplay => P1CostCo.ToString("N0");
        public int P2CostCo { get; set; }
        public string P2CostCoDisplay => P2CostCo.ToString("N0");
    }

    public class NHCoFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class NHCoIndexViewModel : PagedViewModel<NHCoViewModel>
    {

        public NHCoIndexViewModel()
        {
            Filter = new NHCoFilterViewModel();
        }

        public NHCoFilterViewModel Filter { get; set; }

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
        //AddEmptySelectListItem()

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }
}
