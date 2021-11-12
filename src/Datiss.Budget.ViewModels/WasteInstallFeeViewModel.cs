using Datiss.Budget.ViewModels.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Datiss.Budget.ViewModels
{
    public class AddWasteInstallFeeViewModel: BaseViewModel
    {

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int DWasteTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "Please dorost vared kon")]
        public int WInstllFee { get; set; }

        public IEnumerable<SelectListItem> DWasteTypeSource { get; set; }

        public string DWasteTypeTitle {
            get {
                if (DWasteTypeSource == null || !DWasteTypeSource.Any())
                    return string.Empty;

                return DWasteTypeSource.FirstOrDefault(x => x.Value.ToString() == DWasteTypeId.ToString()).Text;
            }
        }
        
    }

    public class UpdateWasteInstallFeeViewModel : AddWasteInstallFeeViewModel
    {
        public int Id { get; set; }

    }

    public class WasteInstallFeeViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int DWasteTypeId { get; set; }
        public string DWasteTypeDisplay { get; set; }
        public int WInstallFee { get; set; }
    }

    public class WasteInstallFeeFilterViewModel: FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? DWasteTypeId { get; set; }
        public int? WInstallFee { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

}
