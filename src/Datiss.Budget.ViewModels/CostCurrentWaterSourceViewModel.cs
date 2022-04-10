using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentWaterSourceViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int WaterSourceTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public int ActiveSource { get; set; }

        [Required(ErrorMessage = "*")]
        public long BaseProduction { get; set; }

        [Required(ErrorMessage = "*")]
        public long LastYearProduction { get; set; }

        [Required(ErrorMessage = "*")]
        public long ForcastProduction { get; set; }

        public IEnumerable<SelectListItem> WaterSourceTypeSource { get; set; }

        public string WaterSourceTypeTitle
        {
            get
            {
                if (WaterSourceTypeSource == null || !WaterSourceTypeSource.Any())
                    return string.Empty;

                return WaterSourceTypeSource.FirstOrDefault(x => x.Value.ToString() == WaterSourceTypeId.ToString()).Text;
            }
        }

    }

    public class UpdateCostCurrentWaterSourceViewModel : CreateCostCurrentWaterSourceViewModel
    {
        public int Id { get; set; }

    }

    public class CostCurrentWaterSourceViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int WaterSourceTypeId { get; set; }
        public string WaterSourceTypeDisplay { get; set; }
        public int ActiveSource { get; set; }
        public string ActiveSourceDisplay => ActiveSource.ToString("N0");
        public long BaseProduction { get; set; }
        public string BaseProductionDisplay => BaseProduction.ToString("N0");
        public long LastYearProduction { get; set; }
        public string LastYearProductionDisplay => LastYearProduction.ToString("N0");
        public long ForcastProduction { get; set; }
        public string ForcastProductionDisplay => ForcastProduction.ToString("N0");
    }

    public class CostCurrentWaterSourceFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? WaterSourceTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostCurrentWaterSourceIndexViewModel : PagedViewModel<CostCurrentWaterSourceViewModel>
    {

        public CostCurrentWaterSourceIndexViewModel()
        {
            Filter = new CostCurrentWaterSourceFilterViewModel();
        }

        public CostCurrentWaterSourceFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> WaterSourceTypeSource { get; set; }


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

        public void SetWaterSourceTypeSource(IEnumerable<DropDownItemViewModel> source)
            => WaterSourceTypeSource = source.Select(x => new SelectListItem
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
