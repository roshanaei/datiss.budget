using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Enum;

namespace Datiss.Budget.ViewModels
{
    public class CreateOrganizationViewModel : BaseViewModel
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public OrganizationType Type { get; set; }

        public bool Enabled { get; set; }

        public bool SewageStatus { get; set; }

        public IList<SelectListItem> ParentList { get; set; }
    }

    public class UpdateOrganizationViewModel : CreateOrganizationViewModel
    {
        public int Id { get; set; }
    }

    public class OrganizationViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public OrganizationType Type { get; set; }

        public bool SewageStatus { get; set; }

        public EntityStatus Status { get; set; }

        public string StatusDisplay => Status.ToDisplay();
    }

    public class OrganizationFilterViewModel : FilterViewModel
    {
        public int? ParentId { get; set; }

        public OrganizationType? Type { get; set; }

        public bool? SewageStatus { get; set; }

        public EntityStatus? Status { get; set; }

        public IList<SelectListItem> ParentOrganozationSource { get; set; }

        public IList<SelectListItem> OrganizationTypeSource { get; set; }

        public IList<SelectListItem> OrganizationStatusSource { get; set; }
    }

    public class OrganizationIndexViewModel : PagedViewModel<OrganizationViewModel>
    {
        public OrganizationIndexViewModel() {
            Filter = new OrganizationFilterViewModel();
        }

        public OrganizationFilterViewModel Filter { get; set; }

        public void SetParentOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectParentOrgId = null) {
            Filter.ParentOrganozationSource = source.Select(x => new SelectListItem {
                Selected = x.Id == selectParentOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }

        public void SetOrganizationTypeFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectOrgTypeId = null) {
            Filter.OrganizationTypeSource = source.Select(x => new SelectListItem {
                Selected = x.Id == selectOrgTypeId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }

        public void SetOrganizationStatusFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectOrgStatusId = null) {
            Filter.OrganizationStatusSource = source.Select(x => new SelectListItem {
                Selected = x.Id == selectOrgStatusId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }

    }
}
