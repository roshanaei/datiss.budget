using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.ViewModels.Base;

namespace Datiss.Budget.ViewModels
{
    public class AddWaterSalesSplitViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int WPipeDiameterId { get; set; }

        [Required(ErrorMessage ="*")]
        [Range(0,int.MaxValue,ErrorMessage ="Please Dorost Vared Kon")]
        public int NumberSales { get; set; }

        [Required(ErrorMessage ="*")]
        [Range(0,int.MaxValue,ErrorMessage ="Please Dorost Vared Kon")]
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

    public class UpdateWaterSalesSplitViewModel :AddWaterSalesSplitViewModel
       {
        public int Id { get; set; }
       }


    public class WaterSalesSplitViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public String OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public String UserTypeDisplay { get; set; }
        public int  WPipeDiameterId { get; set; }
        public string WPipeDiameterDisplay { get; set; }
        public int NumberSales { get; set; }
        public int UnitSales { get; set; }
    }

    public class WaterSalesSplitFilterViewModel : FilterViewModel
    {
        public int? UserTypeId { get; set; }

        public int? WPipeDiameterId { get; set; }

        public int? NumberSales { get; set; }

        public int? UnitSales { get; set; }

        public IEnumerable<SelectListItem> YearSource { get; set; }

        public IEnumerable<SelectListItem> OrganizationSource { get; set; }

    }
}
