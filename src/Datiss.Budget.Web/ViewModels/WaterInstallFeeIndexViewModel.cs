using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class WaterInstallFeeIndexViewModel
    {
        public WaterInstallFeeIndexViewModel() {
            Model = new PagedResult<WaterInstallFeeViewModel>();
            Filter = new WaterInstallFeeFilterViewModel();
        }

        public PagedResult<WaterInstallFeeViewModel> Model { get; set; }

        public WaterInstallFeeFilterViewModel Filter { get; set; }

        public IFormFile ExcelFile { get; set; }

        public void SetOrganizationFilterSource(IEnumerable<DropDownItem> source, int? selectedOrgId = null) {
            Filter.OrganizationSource = source.Select(x => new SelectListItem {
                Selected = x.Id == selectedOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItem> source, int? selectedYearId = null) {
            Filter.YearSource = source.Select(x => new SelectListItem {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }
    }
}
