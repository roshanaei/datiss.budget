using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateDataEntryTimeLimitDTO
    {
        public int? OrganizationId { get; set; }

        public int? YearId { get; set; }

        public int? UserId { get; set; }

        public int? RoleId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string Description { get; set; }
    }
}
