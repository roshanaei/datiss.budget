using Datiss.Budget.ViewModels.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Datiss.Budget.ViewModels
{
   public class AddBranchFeeAmountViewModel:BaseViewModel
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

    public class UpdateBranchFeeAmountViewModel : AddBranchFeeAmountViewModel
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

        public IEnumerable<SelectListItem> YearSource { get; set; }

        public IEnumerable<SelectListItem> OrganizationSource { get; set; }
    }
}
