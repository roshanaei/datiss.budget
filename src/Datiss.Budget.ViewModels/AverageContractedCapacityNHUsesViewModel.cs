using Datiss.Budget.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateAverageContractedCapacityNHUsesViewModel : BaseViewModel 
    {
        public int YearId { get; set; }

        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public decimal AverageCapacity { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public decimal AverageCapacityWs { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public decimal AverageCapacityIncome { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "isNumber")]
        public decimal AverageCapacityWsIncome { get; set; }

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
    }

    public class UpdateAverageContractedCapacityNHUsesViewModel : CreateAverageContractedCapacityNHUsesViewModel
    {
        public int Id { get; set; }
    }

    public class AverageContractedCapacityNHUsesViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public decimal AverageCapacity { get; set; }

        public string AverageCapacityDisplay => AverageCapacity.ToString("N2");

        public decimal AverageCapacityWs { get; set; }

        public string AverageCapacityWsDisplay => AverageCapacityWs.ToString("N2");

        public decimal AverageCapacityIncome { get; set; }

        public string AverageCapacityIncomeDisplay => AverageCapacityIncome.ToString("N2");

        public decimal AverageCapacityWsIncome { get; set; }

        public string AverageCapacityWsIncomeDisplay => AverageCapacityWsIncome.ToString("N2");
    }

    public class AverageContractedCapacityNHUsesFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class AverageContractedCapacityNHUsesIndexViewModel : PagedViewModel<AverageContractedCapacityNHUsesViewModel>
    {
        public AverageContractedCapacityNHUsesIndexViewModel()
        {
            Filter = new AverageContractedCapacityNHUsesFilterViewModel();
        }

        public AverageContractedCapacityNHUsesFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> UserTypeSource { get; set; }

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

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
                => Filter.YearSource = source.Select(x => new SelectListItem
                {
                    Selected = x.Id == selectedYearId,
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
    }
}
