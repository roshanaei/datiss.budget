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
    public class CreateSubscriptionViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public string YearDisplay { get; set; }

        public int UserTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "لطفاً آبونمان آب را بصورت صحیح وارد نمایید.")]
        public int SubW { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "لطفاً آبونمان فاضلاب را بصورت صحیح وارد نمایید.")]
        public int SubWs { get; set; }

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

    public class UpdateSubscriptionViewModel : CreateSubscriptionViewModel
    {
        public int Id { get; set; }
    }

    public class SubscriptionViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public int SubW { get; set; }

        public string SubWDisplay => SubW.ToString("N0");

        public int SubWs { get; set; }

        public string SubWsDisplay => SubWs.ToString("N0");
    }

    public class SubscriptionFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? UserTypeId { get; set; }

        public int? SubW { get; set; }

        public int? SubWs { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
    }

    public class SubscriptionIndexViewModel : PagedViewModel<SubscriptionViewModel>
    {
        public SubscriptionIndexViewModel()
        {
            Filter = new SubscriptionFilterViewModel();
        }

        public SubscriptionFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> UserTypeSource { get; set; }

        public IFormFile ExcelFile { get; set; }

        public void SetYearSource(IEnumerable<DropDownItemViewModel> source)
            => YearSource = source.Select(x => new SelectListItem
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

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
    }
}
