using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeCurrentNOperationalDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int NOICTypeId { get; set; }

        public long NOICPrice { get; set; }

        public string NOICTypeTitle { get; set; }
    }

    public class UpdateIncomeCurrentNOperationalDTO : CreateIncomeCurrentNOperationalDTO
    {
        public int Id { get; set; }

    }

    public class IncomeCurrentNOperationalDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int NOICTypeId { get; set; }
        public string NOICTypeDisplay { get; set; }
        public long NOICPrice { get; set; }
    }

}
