using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class BranchingRateIncreaseImportModel
    {
        [Column(0, MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public string UserTypeDisplay { get; set; }

        [Column(3, MappingDirections.Both, Letter = "D")]
        public int UserTypeId { get; set; }

        [Column(4, MappingDirections.Both, Letter = "E")]
        public int WaterRateIncrease { get; set; }

        [Column(5, MappingDirections.Both, Letter = "F")]
        public int WasteRateIncrease { get; set; }

        [Column(6, MappingDirections.Both, Letter = "G")]
        public int WastePersentIncrease { get; set; }

        [Column(7, MappingDirections.Both, Letter = "H")]
        public int FixAmountBusiness { get; set; }

        [Column(8, MappingDirections.Both, Letter = "I")]
        public int CapacityFixAmount { get; set; }

        [Column(9, MappingDirections.Both, Letter = "J")]
        public int WaterInstallRateIncrease { get; set; }

        [Column(10, MappingDirections.Both, Letter = "K")]
        public int WsInstalIncrease { get; set; }

        [Column(11, MappingDirections.Both, Letter = "L")]
        public int WaterFixNote2 { get; set; }

        [Column(12, MappingDirections.Both, Letter = "M")]
        public int WasteFixNote2 { get; set; }
    }
}
