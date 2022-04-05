using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentBankFeeViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int CostCenterTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public long BankFeeLastYear { get; set; }

        [Required(ErrorMessage = "*")]
        public long BankFeeForcast { get; set; }

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

    public class UpdateCostCurrentBankFeeViewModel : CreateCostCurrentBankFeeViewModel
    {
        public int Id { get; set; }

    }

    public class CostCurrentBankFeeViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }
        public long BankFeeLastYear { get; set; }
        public string BankFeeLastYearDisplay => BankFeeLastYear.ToString("N0");
        public long BankFeeForcast { get; set; }
        public string BankFeeForcastDisplay => BankFeeForcast.ToString("N0");
    }

    public class CostCurrentBankFeeFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? CostCenterTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostCurrentBankFeeIndexViewModel : PagedViewModel<CostCurrentBankFeeViewModel>
    {

        public CostCurrentBankFeeIndexViewModel()
        {
            Filter = new CostCurrentBankFeeFilterViewModel();
        }

        public CostCurrentBankFeeFilterViewModel Filter { get; set; }

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
