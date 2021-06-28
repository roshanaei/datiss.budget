using Datiss.Budget.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;

namespace Datiss.Budget.ViewModels
{
    public class AddConstantViewModel
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string ConstantKey { get; set; }

        public int DisplayOrder { get; set; }

        public EntityStatus Status { get; set; }
    }

    public class UpdateConstantViewModel: AddConstantViewModel
    {
        public int Id { get; set; }
    }
}
