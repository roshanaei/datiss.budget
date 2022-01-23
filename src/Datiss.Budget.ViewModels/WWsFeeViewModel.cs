using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class WWsFeeViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public int UsageLayerId { get; set; }

        public string UsageLayerDisplay { get; set; }

        public int P1Fee { get; set; }

        public string P1FeeDisplay => P1Fee.ToString("N0");

        public int P2Fee { get; set; }

        public string P2FeeDisplay => P2Fee.ToString("N0");

        public int P1Note3 { get; set; }

        public string P1Note3Display => P1Note3.ToString("N0");

        public int P2Note3 { get; set; }

        public string P2Note3Display => P2Note3.ToString("N0");

        public int P1Note7 { get; set; }

        public string P1Note7Display => P1Note7.ToString("N0");

        public int P2Note7 { get; set; }

        public string P2Note7Display => P2Note7.ToString("N0");
    }
}
