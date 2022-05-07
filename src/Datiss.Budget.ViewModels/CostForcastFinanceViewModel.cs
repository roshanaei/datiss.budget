using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostForcastFinanceViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int FinanceSubjectTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public long RemainingAssets { get; set; }

        [Required(ErrorMessage = "*")]
        public long AssetsCreated6_1 { get; set; }

        [Required(ErrorMessage = "*")]
        public long AssetsCreated6_2 { get; set; }

        [Required(ErrorMessage = "*")]
        public long ForcastAssets_D { get; set; }

        [Required(ErrorMessage = "*")]
        public long TotalAssetsCreated_D { get; set; }

    }

    public class UpdateCostForcastFinanceViewModel : CreateCostForcastFinanceViewModel
    {
        public int Id { get; set; }
    }


    public class CostForcastFinanceViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }
        public int FinanceSubjectTypeId { get; set; }
        public string FinanceSubjectTypeDisplay { get; set; }

        public int RemainingAssets { get; set; }
        public string RemainingAssetsDisplay => RemainingAssets.ToString("N0");

        public long AssetsCreated6_1 { get; set; }
        public string AssetsCreated6_1Display => AssetsCreated6_1.ToString("N0");

        public long AssetsCreated6_2 { get; set; }
        public string AssetsCreated6_2Display => AssetsCreated6_2.ToString("N0");

        public long ForcastAssets_D { get; set; }
        public string ForcastAssets_DDisplay => ForcastAssets_D.ToString("N0");

        public long TotalAssetsCreated_D { get; set; }
        public string TotalAssetsCreated_DDisplay => TotalAssetsCreated_D.ToString("N0");

    }

    public class CostForcastFinanceFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }

        public int? CostCenterTypeId { get; set; }

        public int? FinanceSubjectTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

    }

    public class CostForcastFinanceIndexViewModel : PagedViewModel<CostForcastFinanceViewModel>
    {
        public CostForcastFinanceIndexViewModel()
        {
            Filter = new CostForcastFinanceFilterViewModel();
        }
        public CostForcastFinanceFilterViewModel Filter { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> InputOrganizationSource { get; set; }
        public IList<SelectListItem> CostCenterTypeSource { get; set; }
        public IList<SelectListItem> FinanceSubjectTypeSource { get; set; }
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
        public void SetCostCenterTypeSource(IEnumerable<DropDownItemViewModel> source)
            => CostCenterTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        public void SetFinanceSubjectTypeSource(IEnumerable<DropDownItemViewModel> source)
            => FinanceSubjectTypeSource = source.Select(x => new SelectListItem
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