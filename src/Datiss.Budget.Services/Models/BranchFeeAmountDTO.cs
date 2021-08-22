using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    class CreateBranchFeeAmountDTO
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
}
