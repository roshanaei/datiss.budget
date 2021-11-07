using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Datiss.Budget.ViewModels
{
    public class CreateConstantViewModel
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string ConstantKey { get; set; }

        public int DisplayOrder { get; set; }

        public bool Enabled { get; set; }

        public IEnumerable<SelectListItem> ParentList { get; set; }
    }

    public class UpdateConstantViewModel: CreateConstantViewModel
    {
        public int Id { get; set; }
    }
}
