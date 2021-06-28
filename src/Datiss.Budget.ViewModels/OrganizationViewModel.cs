using Datiss.Budget.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class AddOrganizationViewModel
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsVillage { get; set; }

        public bool Enabled { get; set; }
    }

    public class UpdateOrganizationViewModel : AddOrganizationViewModel
    {
        public int Id { get; set; }
    }
}
