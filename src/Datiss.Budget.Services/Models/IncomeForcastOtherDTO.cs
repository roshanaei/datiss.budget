using Datiss.Budget.Enum;
using Datiss.Budget.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeForcastOtherDTO
    {
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public int OIFTypeId{ get; set; }
        public string OIFTypeTitle { get; set; }
        public ActivityType ActivityId { get; set; }
        public int OIFCount { get; set; }
        public long OIFPrice { get; set; }
    }

    public class UpdateIncomeForcastOtherDTO : CreateIncomeForcastOtherDTO
    {
        public int Id { get; set; }

    }

    public class IncomeForcastOtherDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int OIFTypeId{ get; set; }
        public string OIFTypeDisplay { get; set; }
        public ActivityType ActivityId { get; set; }
        public string ActivityDisplay => ActivityId.ToDisplay();
        public int OIFCount { get; set; }
        public long OIFPrice { get; set; }
    }
}
