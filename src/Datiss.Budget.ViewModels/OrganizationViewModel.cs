using Datiss.Budget.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;

namespace Datiss.Budget.ViewModels
{
    public class AddOrganizationViewModel
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public OrganizationType Type { get; set; }

        public bool Enabled { get; set; }

        public bool SewageStatus { get; set; }

        public IEnumerable<SelectListItem> ParentList { get; set; }
    }

    public class UpdateOrganizationViewModel : AddOrganizationViewModel
    {
        public int Id { get; set; }
    }
}
