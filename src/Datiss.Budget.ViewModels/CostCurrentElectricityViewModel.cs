using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CostCurrentElectricityViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int ElectricityAmount { get; set; }

        public string ElectricityAmountDisplay => ElectricityAmount.ToString("N0");

        public long ElectricityCost { get; set; }

        public string ElectricityCostDisplay => ElectricityCost.ToString("N0");
    }
}
