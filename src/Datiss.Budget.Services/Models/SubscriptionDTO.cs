using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateSubscriptionDTO
    {
        public int YearId { get; set; }
        public int UserTypeId { get; set; }
        public int SubW { get; set; }
        public int SubWs { get; set; }
        public string UserTypeTitle { get; set; }
    }

    public class UpdateSubscriptionDTO : CreateSubscriptionDTO
    {
        public int Id { get; set; }
    }

    public class SubscriptionDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int SubW { get; set; }
        public int SubWs { get; set; }
    }

}
