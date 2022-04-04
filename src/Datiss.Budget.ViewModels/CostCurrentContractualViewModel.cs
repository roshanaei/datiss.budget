using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentContractualViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int CostCenterTypeId { get; set; }

        public string ContractDescription { get; set; }

        public bool ExtensionId { get; set; }


        [Required(ErrorMessage = "*")]
        public long ContractLastYear { get; set; }

        [Required(ErrorMessage = "*")]
        public long ContractForcast { get; set; }

        public IEnumerable<SelectListItem> CostCenterTypeSource { get; set; }

        public string CostCenterTypeTitle
        {
            get
            {
                if (CostCenterTypeSource == null || !CostCenterTypeSource.Any())
                    return string.Empty;

                return CostCenterTypeSource.FirstOrDefault(x => x.Value.ToString() == CostCenterTypeId.ToString()).Text;
            }
        }

    }

    public class UpdateCostCurrentContractualViewModel : CreateCostCurrentContractualViewModel
    {
        public int Id { get; set; }

    }

    public class CostCurrentContractualViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }
        public string ContractDescription { get; set; }
        public bool ExtensionId { get; set; }
        public long ContractLastYear { get; set; }
        public string ContractLastYearDisplay => ContractLastYear.ToString("N0");
        public long ContractForcast { get; set; }
        public string ContractForcastDisplay => ContractForcast.ToString("N0");
    }

    public class CostCurrentContractualFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? CostCenterTypeId { get; set; }
        public bool? ExtensionId { get; set; }
        
        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostCurrentContractualIndexViewModel : PagedViewModel<CostCurrentContractualViewModel>
    {

        public CostCurrentContractualIndexViewModel()
        {
            Filter = new CostCurrentContractualFilterViewModel();
        }

        public CostCurrentContractualFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> CostCenterTypeSource { get; set; }


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

        public void SetCostCenterTypeSource(IEnumerable<DropDownItemViewModel> source)
            => CostCenterTypeSource = source.Select(x => new SelectListItem
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
