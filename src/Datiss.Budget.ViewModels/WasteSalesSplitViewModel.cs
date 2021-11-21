using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class CreateWasteSalesSplitViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int WsPipeDiameterId { get; set; }

        [Required(ErrorMessage ="*")]
        [Range(0,int.MaxValue,ErrorMessage ="تعداد انشعاب باید به صورت عددی وارد شود")] //TODO : use resources instead
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
                return UserTypeSource.FirstOrDefault(x => x.Value.ToString() == UserTypeSource.ToString()).Text;
            }
        }
    }

    public class UpdateWasteSalesSplitViewModel : CreateWasteSalesSplitViewModel
    {
        public int Id { get; set; }
    }
     
    public class WasteSalesSplitViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public int WsPipeDiameterId { get; set; }

        public string WspipeDiameterDisplay { get; set; }

        public int NumberSales { get; set; }

        public int UnitSales { get; set; }
    }

    public class WasteSalesSplitFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }
        
        public int? UserTypeId { get; set; }

        public int? WsPipeDiameterId { get; set; }

        public int? NumberSales { get; set; }

        public int? UnitSales { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class WasteSalesSplitIndexViewModel : PagedViewModel<WasteSalesSplitViewModel>
    {

        public WasteSalesSplitIndexViewModel()
        {
            Filter = new WasteSalesSplitFilterViewModel();
        }
        public WasteSalesSplitFilterViewModel Filter { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
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

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrgId = null)
            => Filter.OrganizationSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();


        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();

    }
}
