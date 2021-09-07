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
        [Column(0)]
        public int YearId { get; set; }

        [Column(1)]
        public int OrganizationId { get; set; }

        [Column(2)]
        public int DWaterTypeId { get; set; }

        [Column(3)]
        public int WInstllFee { get; set; }
    }
}
