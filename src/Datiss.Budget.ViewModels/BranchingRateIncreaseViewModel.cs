using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateBranchingRateIncreaseViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeTitle
        {
            get
            {
                if (UserTypeSource == null || !UserTypeSource.Any())
                    return string.Empty;

                return UserTypeSource.FirstOrDefault(x => x.Value.ToString() == UserTypeId.ToString()).Text;
            }
        }

        [Required(ErrorMessage = "*")]
        public int WaterRateIncrease { get; set; }

        [Required(ErrorMessage = "*")]
        public int WasteRateIncrease { get; set; }

        [Required(ErrorMessage = "*")]
        public int WastePersentIncrease { get; set; }

        [Required(ErrorMessage = "*")]
        public int FixAmountBusiness { get; set; }

        [Required(ErrorMessage = "*")]
        public int CapacityFixAmount { get; set; }

        [Required(ErrorMessage = "*")]
        public int WaterInstallRateIncrease { get; set; }

        [Required(ErrorMessage = "*")]
        public int WsInstalIncrease { get; set; }

        [Required(ErrorMessage = "*")]
        public int WaterFixNote2 { get; set; }

        [Required(ErrorMessage = "*")]
        public int WasteFixNote2 { get; set; }
        public IEnumerable<SelectListItem> UserTypeSource { get; set; }

    }

    public class UpdateBranchingRateIncreaseViewModel : CreateBranchingRateIncreaseViewModel
    {
        public int Id { get; set; }

    }

    public class BranchingRateIncreaseViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int WaterRateIncrease { get; set; }
        public string WaterRateIncreaseDisplay => WaterRateIncrease.ToString("N0");

        public int WasteRateIncrease { get; set; }
        public string WasteRateIncreaseDisplay => WasteRateIncrease.ToString("N0");

        public int WastePersentIncrease { get; set; }
        public string WastePersentIncreaseDisplay => WastePersentIncrease.ToString("N0");

        public int FixAmountBusiness { get; set; }
        public string FixAmountBusinessDisplay => FixAmountBusiness.ToString("N0");

        public int CapacityFixAmount { get; set; }
        public string CapacityFixAmountDisplay => CapacityFixAmount.ToString("N0");

        public int WaterInstallRateIncrease { get; set; }
        public string WaterInstallRateIncreaseDisplay => WaterInstallRateIncrease.ToString("N0");

        public int WsInstalIncrease { get; set; }
        public string WsInstalIncreaseDisplay => WsInstalIncrease.ToString("N0");

        public int WaterFixNote2 { get; set; }
        public string WaterFixNote2Display => WaterFixNote2.ToString("N0");

        public int WasteFixNote2 { get; set; }
        public string WasteFixNote2Display => WasteFixNote2.ToString("N0");

    }

    public class BranchingRateIncreaseFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class BranchingRateIncreaseIndexViewModel : PagedViewModel<BranchingRateIncreaseViewModel>
    {

        public BranchingRateIncreaseIndexViewModel()
        {
            Filter = new BranchingRateIncreaseFilterViewModel();
        }

        public BranchingRateIncreaseFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> UserTypeSource { get; set; }

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

        public void SetInputOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => InputOrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetUserTypeSource(IEnumerable<DropDownItemViewModel> source)
            => UserTypeSource = source.Select(x => new SelectListItem
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
            }).ToList();

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }
}
