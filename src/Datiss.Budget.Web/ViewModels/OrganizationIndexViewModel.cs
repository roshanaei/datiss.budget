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
    public class OrganizationIndexViewModel
    {
        public OrganizationIndexViewModel()
        {
            Model = new PagedResult<OrganizationViewModel>();
            Filter = new OrganizationFilterViewModel();
        }

        public PagedResult<OrganizationViewModel> Model { get; set; }

        public OrganizationFilterViewModel Filter { get; set; }

        public void SetParentOrganizationFilterSource (IEnumerable<DropDownItem> source ,int? selectParentOrgId = null)
        {
            Filter.ParentOrganozationSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectParentOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            });
        }

        public void SetOrganizationTypeFilterSource(IEnumerable<DropDownItem> source,int? selectOrgTypeId = null)
        {
            Filter.OrganizationTypeSource = source.Select(x => new SelectListItem 
            { 
                Selected = x.Id == selectOrgTypeId,
                Text = x.Title,
                Value = x.Id.ToString()
            });
        }

        public void SetOrganizationStatusFilterSource(IEnumerable<DropDownItem> source, int? selectOrgStatusId = null)
        {
            Filter.OrganizationStatusSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectOrgStatusId,
                Text = x.Title,
                Value = x.Id.ToString()
            });
        }

    }
}
