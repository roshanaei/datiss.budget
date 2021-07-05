using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Models;


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

        public void SetOrganizationFilterSource(IEnumerable<DropDownItem> source) {
            Filter.OrganizationSource = source.Select(x => new SelectListItem {
                Selected = x.Selected,
                Text = x.Title,
                Value = x.Id.ToString()
            });
        }

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItem> source) {
            Filter.YearSource = source.Select(x => new SelectListItem {
                Selected = x.Selected,
                Text = x.Title,
                Value = x.Id.ToString()
            });
        }
    }
}
