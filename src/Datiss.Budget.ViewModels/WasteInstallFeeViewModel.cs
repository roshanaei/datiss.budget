using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Datiss.Budget.ViewModels
{
    public class CreateWasteInstallFeeViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int DWasteTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "Please dorost vared kon")] //TODO : use resources
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

    public class UpdateWasteInstallFeeViewModel : CreateWasteInstallFeeViewModel
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

        public IEnumerable<SelectListItem> YearSource { get; set; }

        public IEnumerable<SelectListItem> OrganizationSource { get; set; }
    }

    public class WasteInstallFeeIndexViewModel : PagedViewModel<WasteInstallFeeViewModel>
    {
        public WasteInstallFeeIndexViewModel() {
            Filter = new WasteInstallFeeFilterViewModel();
        }

        public WasteInstallFeeFilterViewModel Filter { get; set; }

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source) {
            Filter.OrganizationSource = source.Select(x => new SelectListItem {
                Selected = x.Selected,
                Text = x.Title,
                Value = x.Id.ToString()
            });
        }

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source) {
            Filter.YearSource = source.Select(x => new SelectListItem {
                Selected = x.Selected,
                Text = x.Title,
                Value = x.Id.ToString()
            });
        }
    }
}
