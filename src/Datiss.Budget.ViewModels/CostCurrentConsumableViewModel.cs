using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CostCurrentConsumableViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int ConsumableTypeId { get; set; }

        public string ConsumableTypeDisplay { get; set; }

        public int ConsumableAmount { get; set; }

        public string ConsumableAmountDisaplay => ConsumableAmount.ToString("N0");

        public long ConsumableCost { get; set; }

        public string ConsumableCostDisaplay => ConsumableCost.ToString("N0");

    }
}
