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
    public class CreateIncomeCurrentNOperationalViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int NOICTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "لطفاً مبلغ را بصورت صحیح وارد نمایید.")]
        public long NOICPrice { get; set; }

        public IEnumerable<SelectListItem> NOICTypeSource { get; set; }

        public string NOICTypeTitle
        {
            get
            {
                if (NOICTypeSource == null || !NOICTypeSource.Any())
                    return string.Empty;

                return NOICTypeSource.FirstOrDefault(x => x.Value.ToString() == NOICTypeId.ToString()).Text;
            }
        }

    }

    public class UpdateIncomeCurrentNOperationalViewModel : CreateIncomeCurrentNOperationalViewModel
    {
        public int Id { get; set; }

    }

    public class IncomeCurrentNOperationalViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int NOICTypeId { get; set; }
        public string NOICTypeDisplay { get; set; }
        public long NOICPrice { get; set; }
        public string NOICPriceDisplay => NOICPrice.ToString("N0");
    }

    public class IncomeCurrentNOperationalFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? NOICTypeId { get; set; }
        public long? NOICPrice { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class IncomeCurrentNOperationalIndexViewModel : PagedViewModel<IncomeCurrentNOperationalViewModel>
    {

        public IncomeCurrentNOperationalIndexViewModel()
        {
            Filter = new IncomeCurrentNOperationalFilterViewModel();
        }

        public IncomeCurrentNOperationalFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> NOICTypeSource { get; set; }

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

        public void SetNOICTypeSource(IEnumerable<DropDownItemViewModel> source)
            => NOICTypeSource = source.Select(x => new SelectListItem
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
