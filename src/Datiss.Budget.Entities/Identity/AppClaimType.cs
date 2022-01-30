using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities.Identity
{
    public class AppClaimType
    {
        public int Id  { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public EntityStatus Status { get; set; }
    }

}
