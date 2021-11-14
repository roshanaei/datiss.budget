using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Datiss.Budget.ViewModels
{
    public class CreateWaterSalesSplitViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int WPipeDiameterId { get; set; }

        [Required(ErrorMessage ="*")]
        [Range(0,int.MaxValue,ErrorMessage ="تعداد انشعاب باید بصورتی عددی وارد شود")]
        public int NumberSales { get; set; }

        [Required(ErrorMessage ="*")]
        [Range(0,int.MaxValue,ErrorMessage ="آحاد انشعاب باید بصورت عددی وارد شود")]
        public int UnitSales { get; set; }

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

        public IEnumerable<SelectListItem> WPipeDiameterTypeSourse { get; set; }

        public string WPipeDiameterTitle
        {
            get
            {
                if (WPipeDiameterTypeSourse == null || !WPipeDiameterTypeSourse.Any())
                    return string.Empty;
                return WPipeDiameterTypeSourse.FirstOrDefault(x => x.Value.ToString() == WPipeDiameterId.ToString()).Text;
            }
        }


    }

    public class UpdateWaterSalesSplitViewModel : CreateWaterSalesSplitViewModel
       {
        public int Id { get; set; }
       }


    public class WaterSalesSplitViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int  WPipeDiameterId { get; set; }
        public string WPipeDiameterDisplay { get; set; }
        public int NumberSales { get; set; }
        public int UnitSales { get; set; }
    }

    public class WaterSalesSplitFilterViewModel : FilterViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int? UserTypeId { get; set; }

        public int? WPipeDiameterId { get; set; }

        public int? NumberSales { get; set; }

        public int? UnitSales { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

    }

    public class WaterSalesSplitIndexViewModel : PagedViewModel<WaterSalesSplitViewModel>
    {

        public WaterSalesSplitIndexViewModel() 
        {
            Filter = new WaterSalesSplitFilterViewModel();
        }

        public WaterSalesSplitFilterViewModel Filter { get; set; }

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source) {
            Filter.OrganizationSource = source.Select(x => new SelectListItem {
                Selected = x.Selected,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source) {
            Filter.YearSource = source.Select(x => new SelectListItem {
                Selected = x.Selected,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }

    }

    
}
