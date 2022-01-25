using Datiss.Budget.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateCofficientViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int CofficientTypeId { get; set; }

        public CofficientsGroup GroupName { get; set; }


        [Required(ErrorMessage = "*")]
        public decimal Fee { get; set; }

        public IEnumerable<SelectListItem> CofficientTypeSource { get; set; }

        public string CofficientTypeTitle
        {
            get
            {
                if (CofficientTypeSource == null || !CofficientTypeSource.Any())
                    return string.Empty;

                return CofficientTypeSource.FirstOrDefault(x => x.Value.ToString() == CofficientTypeId.ToString()).Text;
            }
        }

    }

    public class UpdateCofficientViewModel : CreateCofficientViewModel
    {
        public int Id { get; set; }

    }

    public class CofficientViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CofficientTypeId { get; set; }
        public string CofficientTypeTitle { get; set; }
        public decimal Fee { get; set; }
        public string FeeDisplay => Fee.ToString("N2");
    }

    public class CofficientFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? CofficientTypeId { get; set; }
        public CofficientsGroup? GroupName { get; set; }


        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CofficientIndexViewModel : PagedViewModel<CofficientViewModel>
    {

        public CofficientIndexViewModel()
        {
            Filter = new CofficientFilterViewModel();
        }

        public CofficientFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> CofficientTypeSource { get; set; }

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

        public void SetCofficientTypeSource(IEnumerable<DropDownItemViewModel> source)
            => CofficientTypeSource = source.Select(x => new SelectListItem
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
