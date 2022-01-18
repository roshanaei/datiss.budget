using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class BranchFeeAmountImportModel
    {
        [Column(1, MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }
        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public decimal UrbanAdjustmentFactor { get; set; }

        [Column(3, MappingDirections.Both, Letter = "D")]
        public decimal WasteRateInWater { get; set; }

        [Column(4, MappingDirections.Both, Letter = "E")]
        public int WaterBranchingPerHousing { get; set; }

        [Column(5, MappingDirections.Both, Letter = "F")]
        public int TubingCost { get; set; }

        [Column(6, MappingDirections.Both, Letter = "G")]
        public int WaterPartnershipAmountDomestic { get; set; }

        [Column(7, MappingDirections.Both, Letter = "H")]
        public int WaterPartnershipAmountNDomestic { get; set; }

        [Column(8, MappingDirections.Both, Letter = "I")]
        public int WastePartnershipAmountDomestic { get; set; }

        [Column(9, MappingDirections.Both, Letter = "J")]
        public int WastePartnershipAmountNDomestic { get; set; }

        [Column(10, MappingDirections.Both, Letter = "K")]
        public int FixCostNote11H { get; set; }

        [Column(11, MappingDirections.Both, Letter = "L")]
        public int FixCostNote11NH { get; set; }

        [Column(12, MappingDirections.Both, Letter = "M")]
        public int FixCostNote11HWs { get; set; }

        [Column(13, MappingDirections.Both, Letter = "N")]
        public int FixCostNote11NHWs { get; set; }

        [Column(14, MappingDirections.Both, Letter = "O")]
        public int WsTubingCost { get; set; }

    }
}
