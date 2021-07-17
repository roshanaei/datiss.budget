using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateWasteInstallFeeDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int DWasteTypeId { get; set; }

        public int WInstllFee { get; set; }

        public string DWasteTypeTitle { get; set; }
    }


}
