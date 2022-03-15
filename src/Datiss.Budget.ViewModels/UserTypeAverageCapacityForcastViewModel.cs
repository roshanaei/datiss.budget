using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateUserTypeAverageCapacityForcastViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

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

        [Required(ErrorMessage = "*")]
        public decimal AverageCapacityW { get; set; }

        [Required(ErrorMessage = "*")]
        public decimal AverageCapacityWs { get; set; }

        [Required(ErrorMessage = "*")]
        public decimal AverageCapacityWIncome { get; set; }

        [Required(ErrorMessage = "*")]
        public decimal AverageCapacityWsIncome { get; set; }

    }

    public class UpdateUserTypeAverageCapacityForcastViewModel : CreateUserTypeAverageCapacityForcastViewModel
    {
        public int Id { get; set; }

    }

    public class UserTypeAverageCapacityForcastForcastViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }

        public decimal AverageCapacityW { get; set; }
        public string AverageCapacityWDisplay => AverageCapacityW.ToString("N2");

        public decimal AverageCapacityWs { get; set; }
        public string AverageCapacityWsDisplay => AverageCapacityWs.ToString("N2");

        public decimal AverageCapacityWIncome { get; set; }
        public string AverageCapacityWIncomeDisplay => AverageCapacityWIncome.ToString("N2");

        public decimal AverageCapacityWsIncome { get; set; }
        public string AverageCapacityWsIncomeDisplay => AverageCapacityWsIncome.ToString("N2");
    }

    public class UserTypeAverageCapacityForcastFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class UserTypeAverageCapacityForcastIndexViewModel : PagedViewModel<UserTypeAverageCapacityForcastForcastViewModel>
    {

        public UserTypeAverageCapacityForcastIndexViewModel()
        {
            Filter = new UserTypeAverageCapacityForcastFilterViewModel();
        }

        public UserTypeAverageCapacityForcastFilterViewModel Filter { get; set; }

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
        //AddEmptySelectListItem()

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }

}
