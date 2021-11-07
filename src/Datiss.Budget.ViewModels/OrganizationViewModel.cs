using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Enum;
using Datiss.Budget.ViewModels.Base;

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

        public IEnumerable<SelectListItem> ParentList { get; set; }
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
    }

    public class OrganizationFilterViewModel : FilterViewModel
    {
        public int? ParentId { get; set; }

        public OrganizationType? Type { get; set; }

        public bool? SewageStatus { get; set; }

        public EntityStatus? Status { get; set; }

        public IEnumerable<SelectListItem> ParentOrganozationSource { get; set; }

        public IEnumerable<SelectListItem> OrganizationTypeSource { get; set; }

        public IEnumerable<SelectListItem> OrganizationStatusSource { get; set; }
    }
}
