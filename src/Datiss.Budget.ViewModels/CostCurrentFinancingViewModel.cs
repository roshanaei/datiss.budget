using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentFinancingViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int FinancialCostTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public long BaseFee { get; set; }

        [Required(ErrorMessage = "*")]
        public long LastYearFee { get; set; }

        public IEnumerable<SelectListItem> FinancialCostTypeSource { get; set; }

        public string FinancialCostTypeTitle
        {
            get
            {
                if (FinancialCostTypeSource == null || !FinancialCostTypeSource.Any())
                    return string.Empty;

                return FinancialCostTypeSource.FirstOrDefault(x => x.Value.ToString() == FinancialCostTypeId.ToString()).Text;
            }
        }

    }

    public class UpdateCostCurrentFinancingViewModel : CreateCostCurrentFinancingViewModel
    {
        public int Id { get; set; }
        public long ForcastFee { get; set; }

    }

    public class CostCurrentFinancingViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int FinancialCostTypeId { get; set; }
        public string FinancialCostTypeDisplay { get; set; }
        public long BaseFee { get; set; }
        public string BaseFeeDisplay => BaseFee.ToString("N0");
        public long LastYearFee { get; set; }
        public string LastYearFeeDisplay => LastYearFee.ToString("N0");
        public long ForcastFee { get; set; }
        public string ForcastFeeDisplay => ForcastFee.ToString("N0");
    }

    public class CostCurrentFinancingFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? FinancialCostTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostCurrentFinancingIndexViewModel : PagedViewModel<CostCurrentFinancingViewModel>
    {

        public CostCurrentFinancingIndexViewModel()
        {
            Filter = new CostCurrentFinancingFilterViewModel();
        }

        public CostCurrentFinancingFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }


        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> FinancialCostTypeSource { get; set; }


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

        public void SetFinancialCostTypeSource(IEnumerable<DropDownItemViewModel> source)
            => FinancialCostTypeSource = source.Select(x => new SelectListItem
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
