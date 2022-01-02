using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateIncomeForcastViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public IEnumerable<SelectListItem> UserTypeSource { get; set; }
        //public string DWaterTypeTitle
        //{
        //    get
        //    {
        //        if (DUserTypeSource == null || !DUserTypeSource.Any())
        //            return string.Empty;

        //        return DUserTypeSource.FirstOrDefault(x => x.Value.ToString() == DUserTypeId.ToString()).Text;
        //    }
        //}
        [Required(ErrorMessage = "*")]
        public int NumberUser { get; set; }
        [Required(ErrorMessage = "*")]
        public int UnitUser { get; set; }
        [Required(ErrorMessage = "*")]
        public int WaterInstllIncome { get; set; }
        [Required(ErrorMessage = "*")]
        public int WaterBranchIncome { get; set; }
        [Required(ErrorMessage = "*")]
        public int WaterNote2Income { get; set; }
        [Required(ErrorMessage = "*")]
        public int WaterNote3Income { get; set; }
        [Required(ErrorMessage = "*")]
        public int WNote11Income { get; set; }
    }
    public class UpdateIncomeForcastViewModel : CreateIncomeForcastViewModel
    {
        public int Id { get; set; }
    }
    public class IncomeForcastViewModel
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
        public int WaterInstllIncome { get; set; }
        public string WaterInstllIncomeDisplay => WaterInstllIncome.ToString("N0");
        public int WaterBranchIncome { get; set; }
        public string WaterBranchIncomeDisplay => WaterBranchIncome.ToString("N0");
        public int WaterNote2Income { get; set; }
        public string WaterNote2IncomeDisplay => WaterNote2Income.ToString("N0");
        public int WaterNote3Income { get; set; }
        public string WaterNote3IncomeDisplay => WaterNote3Income.ToString("N0");
        public int WNote11Income { get; set; }
        public string WNote11IncomeDisplay => WNote11Income.ToString("N0");
    }
    public class IncomeForcastFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
    }
    public class IncomeForcastIndexViewModel : PagedViewModel<IncomeForcastViewModel>
    {
        public IncomeForcastIndexViewModel()
        {
            Filter = new IncomeForcastFilterViewModel();
        }

        public IncomeForcastFilterViewModel Filter { get; set; }
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
