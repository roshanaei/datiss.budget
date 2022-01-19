using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

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
        public string UrbanAdjustmentFactorDisplay => UrbanAdjustmentFactor.ToString("N2");


        public decimal WasteRateInWater { get; set; }
        public string WasteRateInWaterDisplay => WasteRateInWater.ToString("N2");


        public int WaterBranchingPerHousing { get; set; }
        public string WaterBranchingPerHousingDisplay => WaterBranchingPerHousing.ToString("N0");


        public int TubingCost { get; set; }
        public string TubingCostDisplay => TubingCost.ToString("N0");


        public int WaterPartnershipAmountDomestic { get; set; }
        public string WaterPartnershipAmountDomesticDisplay => WaterPartnershipAmountDomestic.ToString("N0");


        public int WaterPartnershipAmountNDomestic { get; set; }
        public string WaterPartnershipAmountNDomesticDisplay => WaterPartnershipAmountNDomestic.ToString("N0");


        public int WastePartnershipAmountDomestic { get; set; }
        public string WastePartnershipAmountDomesticDisplay => WastePartnershipAmountDomestic.ToString("N0");


        public int WastePartnershipAmountNDomestic { get; set; }
        public string WastePartnershipAmountNDomesticDisplay => WastePartnershipAmountNDomestic.ToString("N0");


        public int FixCostNote11H { get; set; }
        public string FixCostNote11HDisplay => FixCostNote11H.ToString("N0");


        public int FixCostNote11NH { get; set; }
        public string FixCostNote11NHDisplay => FixCostNote11NH.ToString("N0");


        public int FixCostNote11HWs { get; set; }
        public string FixCostNote11HWsDisplay => FixCostNote11HWs.ToString("N0");


        public int FixCostNote11NHWs { get; set; }
        public string FixCostNote11NHWsDisplay => FixCostNote11NHWs.ToString("N0");


        public int WsTubingCost { get; set; }
        public string WsTubingCostDisplay => WsTubingCost.ToString("N0");

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

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

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

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source,int? selectedOrgId = null) {
            Filter.OrganizationSource = source.Select(x => new SelectListItem {
                Selected = x.Selected,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        }

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrgId = null) {
            Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Selected,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        }
    }
}
