using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateIncomeForcastWsViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public IEnumerable<SelectListItem> UserTypeSource { get; set; }
        [Required(ErrorMessage = "*")]
        public int NumberUser { get; set; }
        [Required(ErrorMessage = "*")]
        public int UnitUser { get; set; }
        [Required(ErrorMessage = "*")]
        public int WasteInstallIncome { get; set; }
        [Required(ErrorMessage = "*")]
        public int WasteBranchIncome { get; set; }
        [Required(ErrorMessage = "*")]
        public int WasteNote3Income { get; set; }
        [Required(ErrorMessage = "*")]
        public int WsNote11Income { get; set; }
    }
    public class UpdateIncomeForcastWsViewModel : CreateIncomeForcastWsViewModel
    {
        public int Id { get; set; }
    }
    public class IncomeForcastWsViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int NumberUser { get; set; }
        public string NumberUserDisplay => NumberUser.ToString("N0");
        public int UnitUser { get; set; }
        public string UnitUserDisplay => UnitUser.ToString("N0");
        public int WasteInstallIncome { get; set; }
        public string WasteInstallIncomeDisplay => WasteInstallIncome.ToString("N0");
        public int WasteBranchIncome { get; set; }
        public string WasteBranchIncomeDisplay => WasteBranchIncome.ToString("N0");
        public int WasteNote3Income { get; set; }
        public string WasteNote3IncomeDisplay => WasteNote3Income.ToString("N0");
        public int WsNote11Income { get; set; }
        public string WsNote11IncomeDisplay => WsNote11Income.ToString("N0");
    }
    public class IncomeForcastWsFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
    }
    public class IncomeForcastWsIndexViewModel : PagedViewModel<IncomeForcastWsViewModel>
    {
        public IncomeForcastWsIndexViewModel()
        {
            Filter = new IncomeForcastWsFilterViewModel();
        }

        public IncomeForcastWsFilterViewModel Filter { get; set; }
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
