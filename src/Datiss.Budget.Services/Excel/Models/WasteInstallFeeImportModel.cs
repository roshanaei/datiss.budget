using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class WasteInstallFeeImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string DWasteTypeTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int DWasteTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int WsInstallFee { get; set; }
    }
}
