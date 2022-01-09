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
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string UserTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int UserTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int WaterRateIncrease { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int WasteRateIncrease { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int WastePersentIncrease { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int FixAmountBusiness { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int CapacityFixAmount { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public int WaterInstallRateIncrease { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public int WsInstalIncrease { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public int WaterFixNote2 { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public int WasteFixNote2 { get; set; }
    }
}
