using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeCurrentOperationalDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ICOTypeId { get; set; }

        public int CountH { get; set; }

        public long PriceH { get; set; }

        public long CostH { get; set; }

        public int CountNH { get; set; }

        public long PriceNH { get; set; }

        public long CostNH { get; set; }

        public int TotalCount { get; set; }

        public long TotalCost { get; set; }

        public string ICOTypeDisplay { get; set; }
    }

    public class UpdateIncomeCurrentOperationalDTO : CreateIncomeCurrentOperationalDTO
    {
        public int Id { get; set; }
    }

    public class IncomeCurrentOperationalDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay { get; set; }

        public int ICOTypeId { get; set; }

        public string ICOTypeDisplay { get; set; }

        public int CountH { get; set; }

        public long PriceH { get; set; }

        public long CostH { get; set; }

        public int CountNH { get; set; }

        public long PriceNH { get; set; }

        public long CostNH { get; set; }

        public int TotalCount { get; set; }

        public long TotalCost { get; set; }
    }
}
