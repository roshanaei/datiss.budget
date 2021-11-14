using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Datiss.Budget.ViewModels
{
    public class CreateBranchFeeAmountViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public decimal UrbanAdjustmentFactor { get; set; }

        public decimal WasteRateInWater { get; set; }

        public int WaterBranchingPerHousing { get; set; }

        public int TubingCost { get; set; }

        public int WaterPartnershipAmountDomestic { get; set; }

        public int WaterPartnershipAmountNDomestic { get; set; }

        public int WastePartnershipAmountDomestic { get; set; }

        public int WastePartnershipAmountNDomestic { get; set; }

        public int FixCostNote11H { get; set; }

        public int FixCostNote11NH { get; set; }

        public int FixCostNote11HWs { get; set; }

        public int FixCostNote11NHWs { get; set; }

        public int WsTubingCost { get; set; }
    }

    public class UpdateBranchFeeAmountViewModel : CreateBranchFeeAmountViewModel
    {
        public int Id { get; set; }
    }

    public class BranchFeeAmountViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public decimal UrbanAdjustmentFactor { get; set; }

        public decimal WasteRateInWater { get; set; }

        public int WaterBranchingPerHousing { get; set; }

        public int TubingCost { get; set; }

        public int WaterPartnershipAmountDomestic { get; set; }

        public int WaterPartnershipAmountNDomestic { get; set; }

        public int WastePartnershipAmountDomestic { get; set; }

        public int WastePartnershipAmountNDomestic { get; set; }

        public int FixCostNote11H { get; set; }

        public int FixCostNote11NH { get; set; }

        public int FixCostNote11HWs { get; set; }

        public int FixCostNote11NHWs { get; set; }

        public int WsTubingCost { get; set; }
    }

    public class BranchFeeAmountFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; } 

        public string OrganizationDisplay { get; set; }

        public decimal? UrbanAdjustmentFactor { get; set; }

        public decimal? WasteRateInWater { get; set; }

        public int? WaterBranchingPerHousing { get; set; }

        public int? TubingCost { get; set; }

        public int? WaterPartnershipAmountDomestic { get; set; }

        public int? WaterPartnershipAmountNDomestic { get; set; }

        public int? WastePartnershipAmountDomestic { get; set; }

        public int? WastePartnershipAmountNDomestic { get; set; }

        public int? FixCostNote11H { get; set; }

        public int? FixCostNote11NH { get; set; }

        public int? FixCostNote11HWs { get; set; }

        public int? FixCostNote11NHWs { get; set; }

        public int? WsTubingCost { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class BranchFeeAmountIndexViewModel : PagedViewModel<BranchFeeAmountViewModel>
    {
        public BranchFeeAmountIndexViewModel() {
            Filter = new BranchFeeAmountFilterViewModel();
        }

        public BranchFeeAmountFilterViewModel Filter { get; set; }

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source) {
            Filter.OrganizationSource = source.Select(x => new SelectListItem {
                Selected = x.Selected,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source) {
            Filter.YearSource = source.Select(x => new SelectListItem {
                Selected = x.Selected,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }
    }
}
