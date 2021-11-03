
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;
using Datiss.Budget.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.ViewModels.Base;

namespace Datiss.Budget.ViewModels
{
    public class AddConstantViewModel : BaseViewModel
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string ConstantKey { get; set; }

        public int DisplayOrder { get; set; }

        public bool Enabled { get; set; }

        public IEnumerable<SelectListItem> ParentList { get; set; }
    }

    public class UpdateConstantViewModel: AddConstantViewModel
    {
        public int Id { get; set; }
    }

    public class ConstantViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string ConstantKey { get; set; }

        public int DisplayOrder { get; set; }

        public bool Enabled { get; set; }

    }

    public class ConstantFilterViewModel : FilterViewModel
    {

        public int? ParentId { get; set; }

        public IEnumerable<SelectListItem> ParentSource { get; set; }
    }
}
