using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class CreateWaterInstallFeeViewModel: BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int DWaterTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "لطفاً مبلغ را بصورت صحیح وارد نمایید.")]
        public int WInstallFee { get; set; }

        public IEnumerable<SelectListItem> DWaterTypeSource { get; set; }

        public string DWaterTypeTitle {
            get {
                if (DWaterTypeSource == null || !DWaterTypeSource.Any())
                    return string.Empty;

                return DWaterTypeSource.FirstOrDefault(x => x.Value.ToString() == DWaterTypeId.ToString()).Text;
            }
        }
        
    }

    public class UpdateWaterInstallFeeViewModel : CreateWaterInstallFeeViewModel
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
        public string WInstallFeeDisplay => WInstallFee.ToString("N0");
    }

    public class WaterInstallFeeFilterViewModel: FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? DWaterTypeId { get; set; }
        public int? WInstallFee { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class WaterInstallFeeIndexViewModel : PagedViewModel<WaterInstallFeeViewModel> 
    {

        public WaterInstallFeeIndexViewModel() {
            Filter = new WaterInstallFeeFilterViewModel();
        }

        public WaterInstallFeeFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        //public string YearSourceArray => YearSource.ToStringArray();

        public IList<SelectListItem> OrganizationSource { get; set; }

        //public string OrganizationSourceArray => OrganizationSource.ToStringArray();
        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> DWaterTypeSource { get; set; }

        //public string DWaterTypeSourceArray => DWaterTypeSource.ToStringArray();

        public IFormFile ExcelFile { get; set; }

        public void SetYearSource(IEnumerable<DropDownItemViewModel> source) 
            => YearSource = source.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        
        public void SetOrganizationSource(IEnumerable<DropDownItemViewModel> source) 
            => OrganizationSource = source.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetInputOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => InputOrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetDWaterTypeSource(IEnumerable<DropDownItemViewModel> source) 
            => DWaterTypeSource = source.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrgId = null) 
            => Filter.OrganizationSource = source.Select(x => new SelectListItem {
                Selected = x.Id == selectedOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        //AddEmptySelectListItem()

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null) 
            => Filter.YearSource = source.Select(x => new SelectListItem {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        
    }

}
