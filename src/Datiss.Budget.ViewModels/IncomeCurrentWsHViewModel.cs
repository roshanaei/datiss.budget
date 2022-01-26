using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class IncomeCurrentWsHViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public int UsageLayerId { get; set; }

        public string UsageLayerDisplay { get; set; }

        public int NumberUser { get; set; }

        public string NumberUserDisplay => NumberUser.ToString("N0");

        public int UnitUser { get; set; }

        public string UnitUserDisplay => UnitUser.ToString("N0");

        public decimal AvgConsumeUser { get; set; }

        public string AvgConsumeUserDisplay => AvgConsumeUser.ToString("N2");

        public int ConsumptionUser { get; set; }

        public string ConsumptionUserDisplay => ConsumptionUser.ToString("N0");

        public int Cost { get; set; }

        public string CostDisplay => Cost.ToString("N0");

        public int Income { get; set; }

        public string IncomeDisplay => Income.ToString("N0");

        public int SubscriptionIncome { get; set; }

        public string SubscriptionIncomeDisplay => SubscriptionIncome.ToString("N0");

        public int Note3Price { get; set; }

        public string Note3PriceDisplay => Note3Price.ToString("N0");

        public int Note3Income { get; set; }

        public string Note3IncomeDisplay => Note3Income.ToString("N0");

        public int SeasonalIncome { get; set; }

        public string SeasonalIncomeDisplay => SeasonalIncome.ToString("N0");

        public int TIncome { get; set; }

        public string TIncomeDisplay => TIncome.ToString("N0");

        public int Note7Price { get; set; }

        public string Note7PriceDisplay { get; set; }

        public int Note7Income { get; set; }

        public string Note7IncomeDisplay { get; set; }
    }
}
