using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class WaterInstallFeeImportModel
    {
        [Column(0, MappingDirections.Both, Letter = "A")]
        public int YearId { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public int DWaterTypeId { get; set; }

        [Column(3, MappingDirections.Both, Letter = "D")]
        public int WInstllFee { get; set; }
    }
}
