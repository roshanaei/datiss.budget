using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentOtherCofficientViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int CCOtherCostsTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public Decimal Fee { get; set; }

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

    public class UpdateCostCurrentOtherCofficientViewModel : CreateCostCurrentOtherCofficientViewModel
    {
        public int Id { get; set; }
    }


    public class CostCurrentOtherCofficientViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }
        public int CCOtherCostsTypeId { get; set; }
        public string CCOtherCostsTypeDisplay { get; set; }
        public decimal Fee { get; set; }
    }

    public class CostCurrentOtherCofficientFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? CostCenterTypeId { get; set; }

        public int? CCOtherCostsTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

    }

    public class CostCurrentOtherCofficientIndexViewModel : PagedViewModel<CostCurrentOtherCofficientViewModel>
    {
        public CostCurrentOtherCofficientIndexViewModel()
        {
            Filter = new CostCurrentOtherFilterViewModel();
        }
        public CostCurrentOtherFilterViewModel Filter { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> InputOrganizationSource { get; set; }
        public IList<SelectListItem> CostCenterTypeSource { get; set; }
        public IList<SelectListItem> CCOtherCostsTypeSource { get; set; }
        public IFormFile ExcelFile { get; set; }
        public void SetYearSource(IEnumerable<DropDownItemViewModel> source)
            => YearSource = source.Select(x => new SelectListItem
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
        public void SetCCOtherCostsTypeSource(IEnumerable<DropDownItemViewModel> source)
            => CCOtherCostsTypeSource = source.Select(x => new SelectListItem
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

    }


}