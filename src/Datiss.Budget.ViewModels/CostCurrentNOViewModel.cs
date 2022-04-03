using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentNOViewModelViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int CostCurrentNoTypeId { get; set; }


        [Required(ErrorMessage = "*")]
        public long BaseFee { get; set; }
        
        [Required(ErrorMessage = "*")]
        public long LastYearFee { get; set; }


        public IEnumerable<SelectListItem> CostCurrentNOTypeSource { get; set; }

        public string CostCurrentNOTypeTitle
        {
            get
            {
                if(CostCurrentNOTypeSource == null || !CostCurrentNOTypeSource.Any())
                    return string.Empty;

                return CostCurrentNOTypeSource.FirstOrDefault(x => x.Value.ToString() == CostCurrentNoTypeId.ToString()).Text;
            }
        }

    }

    public class UpdateCostCurrentNOViewModel : CreateCostCurrentNOViewModelViewModel
    {
        public int Id { get; set; }

        public long ForcastFee { get; set; }
    }

    public class CostCurrentNOViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int CostCurrentNOTypeId { get; set; }

        public string CostCurrentNOTypeTitle { get; set; }
        
        public long BaseFee { get; set; }

        public string BaseFeeDisplay => BaseFee.ToString("N0");

        public long LastYearFee { get; set; }

        public string LastYearFeeDisplay => LastYearFee.ToString("N0");

        public long ForcastFee { get; set; }

        public string ForcastFeeDisplay => ForcastFee.ToString("N0");

    }

    public class CostCurrentNOFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set;}

        public int? CostCurrentNOTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostCurrentNOIndexViewModel : PagedViewModel<CostCurrentNOViewModel>
    {
        public CostCurrentNOIndexViewModel()
        {
            Filter = new CostCurrentNOFilterViewModel();
        }

        public CostCurrentNOFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> CostCurrentTypeSource { get; set; }

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

        public void SetCostCurrentTypeSource(IEnumerable<DropDownItemViewModel> source)
            => CostCurrentTypeSource = source.Select(x => new SelectListItem
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
