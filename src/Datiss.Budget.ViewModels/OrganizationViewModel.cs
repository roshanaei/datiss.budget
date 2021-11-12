using Datiss.Budget.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;
using Datiss.Budget.ViewModels.Base;

namespace Datiss.Budget.ViewModels
{
    public class AddOrganizationViewModel : BaseViewModel
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public OrganizationType Type { get; set; }

        public bool Enabled { get; set; }

        public bool SewageStatus { get; set; }

        public IList<SelectListItem> ParentList { get; set; }
    }

    public class UpdateOrganizationViewModel : AddOrganizationViewModel
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

        public IList<SelectListItem> ParentOrganozationSource { get; set; }

        public IList<SelectListItem> OrganizationTypeSource { get; set; }

        public IList<SelectListItem> OrganizationStatusSource { get; set; }
    }
}
