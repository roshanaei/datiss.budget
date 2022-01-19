using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateSalesSplitTotalViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public int WNumber { get; set; }

        [Required(ErrorMessage = "*")]
        public int WUnit { get; set; }

        [Required(ErrorMessage = "*")]
        public int WsNumber { get; set; }

        [Required(ErrorMessage = "*")]
        public int WsUnit { get; set; }

        [Required(ErrorMessage = "*")]
        public int WNumber_2 { get; set; }

        [Required(ErrorMessage = "*")]
        public int WUnit_2 { get; set; }

        [Required(ErrorMessage = "*")]
        public int WsNumber_2 { get; set; }

        [Required(ErrorMessage = "*")]
        public int WsUnit_2 { get; set; }

        public IEnumerable<SelectListItem> UserTypeSource { get; set; }

        public string UserTypeTitle
        {
            get
            {
                if (UserTypeSource == null || !UserTypeSource.Any())
                    return string.Empty;
                return UserTypeSource.FirstOrDefault(x => x.Value.ToString() == UserTypeId.ToString()).Text;
            }
        }
    }

    public class UpdateSalesSplitTotalViewModel : CreateSalesSplitTotalViewModel
    {
        public int Id { get; set; }
    }


    public class SalesSplitTotalViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int WNumber { get; set; }
        public string WNumberDisplay => WNumber.ToString("N0");
        public int WUnit { get; set; }
        public string WUnitDisplay => WUnit.ToString("N0");

        public int WsNumber { get; set; }
        public string WsNumberDisplay => WsNumber.ToString("N0");

        public int WsUnit { get; set; }
        public string WsUnitDisplay => WsUnit.ToString("N0");

        public int WNumber_2 { get; set; }
        public string WNumber_2Display => WNumber_2.ToString("N0");

        public int WUnit_2 { get; set; }
        public string WUnit_2Display => WUnit_2.ToString("N0");

        public int WsNumber_2 { get; set; }
        public string WsNumber_2Display => WsNumber_2.ToString("N0");

        public int WsUnit_2 { get; set; }
        public string WsUnit_2Display => WsUnit_2.ToString("N0");

    }

    public class SalesSplitTotalFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }

        public int? UserTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

    }

    public class SalesSplitTotalIndexViewModel : PagedViewModel<SalesSplitTotalViewModel>
    {
        public SalesSplitTotalIndexViewModel()
        {
            Filter = new SalesSplitTotalFilterViewModel();
        }
        public SalesSplitTotalFilterViewModel Filter { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> InputOrganizationSource { get; set; }
        public IList<SelectListItem> UserTypeSource { get; set; }

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
