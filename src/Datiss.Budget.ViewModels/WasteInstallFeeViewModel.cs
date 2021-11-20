using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

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
        public int WsInstallFee { get; set; }
    }

    public class WasteInstallFeeFilterViewModel: FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? DWasteTypeId { get; set; }
        public int? WsInstallFee { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class WasteInstallFeeIndexViewModel : PagedViewModel<WasteInstallFeeViewModel>
    {
        public WasteInstallFeeIndexViewModel() {
            Filter = new WasteInstallFeeFilterViewModel();
        }

        public WasteInstallFeeFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        //public string YearSourceArray => YearSource.ToStringArray();

        public IList<SelectListItem> OrganizationSource { get; set; }

        //public string OrganizationSourceArray => OrganizationSource.ToStringArray();

        public IList<SelectListItem> DWaterTypeSource { get; set; }

        //public string DWaterTypeSourceArray => DWaterTypeSource.ToStringArray();

        public IFormFile ExcelFile { get; set; }

        public void SetYearSource(IEnumerable<DropDownItemViewModel> source)
            => YearSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => OrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();


        public void SetDWaterTypeSource(IEnumerable<DropDownItemViewModel> source)
            => DWaterTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrgId = null)
            => Filter.OrganizationSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();


        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
    }
    
}
