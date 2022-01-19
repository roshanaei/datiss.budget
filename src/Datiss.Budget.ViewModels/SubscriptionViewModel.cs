using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public int SubW { get; set; }

        public string SubWDisplay => SubW.ToString("N0");

        public int SubWs { get; set; }

        public string SubWsDisplay => SubWs.ToString("N0");

    }
}
