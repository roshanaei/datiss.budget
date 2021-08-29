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
    public class AddWaterInstallFeeViewModel: BaseViewModel
    {

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int DWaterTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "لطفاً مبلغ را بصورت صحیح وارد نمایید.")]
        public int WInstllFee { get; set; }

        public IEnumerable<SelectListItem> DWaterTypeSource { get; set; }

        public string DWaterTypeTitle {
            get {
                if (DWaterTypeSource == null || !DWaterTypeSource.Any())
                    return string.Empty;

                return DWaterTypeSource.FirstOrDefault(x => x.Value.ToString() == DWaterTypeId.ToString()).Text;
            }
        }
        
    }

    public class UpdateWaterInstallFeeViewModel : AddWaterInstallFeeViewModel
    {
        public int Id { get; set; }

    }

    public class WaterInstallFeeViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int DWaterTypeId { get; set; }
        public string DWaterTypeDisplay { get; set; }
        public int WInstallFee { get; set; }
    }

    public class WaterInstallFeeFilterViewModel: FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? DWaterTypeId { get; set; }
        public int? WInstallFee { get; set; }

        public IEnumerable<SelectListItem> YearSource { get; set; }

        public IEnumerable<SelectListItem> OrganizationSource { get; set; }
    }

}
