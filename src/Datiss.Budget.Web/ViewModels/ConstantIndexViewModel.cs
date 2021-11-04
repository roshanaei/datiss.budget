using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Web.ViewModels
{
    public class ConstantIndexViewModel
    {
        public ConstantIndexViewModel()
        {
            Model = new PagedResult<ConstantViewModel>();
            Filter = new ConstantFilterViewModel();
        }

        public PagedResult<ConstantViewModel> Model { get; set; }

        public ConstantFilterViewModel Filter { get; set; }

        public void SetConstantParentSource(IEnumerable<DropDownItem> source, int? selectedParentId = null) {
            Filter.ParentSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedParentId,
                Text = x.Title,
                Value = x.Id.ToString()
            });
        }
    }
}
