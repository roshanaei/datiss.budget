using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateWaterInstallFeeDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int DWaterTypeId { get; set; }

        public int WInstllFee { get; set; }

        public string DWaterTypeTitle { get; set; }
    }


}
