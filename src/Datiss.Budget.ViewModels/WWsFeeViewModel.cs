using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateWWsFeeViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int UserTypeId { get; set; }

        public int UsageLayerId { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "پارامتر اول تعرفه بهای خدمات باید به صورت عددی وارد شود")]
        public int P1Fee { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "پارامتر دوم تعرفه بهای خدمات باید به صورت عددی وارد شود")]
        public int P2Fee { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "پارامتر اول خدمات تبصره 3 باید به صورت عددی وارد شود")]
        public int P1Note3 { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "پارامتر دوم خدمات تبصره 3 باید به صورت عددی وارد شود")]
        public int P2Note3 { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "پارامتر اول خدمات تبصره 7 باید به صورت عددی وارد شود")]
        public int P1Note7 { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "پارامتر اول خدمات تبصره 7 باید به صورت عددی وارد شود")]
        public int P2Note7 { get; set; }

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

        public IEnumerable<SelectListItem> ActivityTypeSource
            => EnumSelectListProvider.GetActivityTypeItems(ActivityType);
    }

    public class WWsFeeViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public int UsageLayerId { get; set; }

        public string UsageLayerDisplay { get; set; }

        public int P1Fee { get; set; }

        public string P1FeeDisplay => P1Fee.ToString("N0");

        public int P2Fee { get; set; }

        public string P2FeeDisplay => P2Fee.ToString("N0");

        public int P1Note3 { get; set; }

        public string P1Note3Display => P1Note3.ToString("N0");

        public int P2Note3 { get; set; }

        public string P2Note3Display => P2Note3.ToString("N0");

        public int P1Note7 { get; set; }

        public string P1Note7Display => P1Note7.ToString("N0");

        public int P2Note7 { get; set; }

        public string P2Note7Display => P2Note7.ToString("N0");
    }
}
