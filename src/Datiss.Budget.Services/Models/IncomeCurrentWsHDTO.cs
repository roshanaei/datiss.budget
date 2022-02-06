using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeCurrentWsHDTO 
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public int UsageLayerId { get; set; }

        public string UsageLayerTitle { get; set; }

        public int NumberUser { get; set; }

        public int UnitUser { get; set; }

        public decimal AvgConsumeUser { get; set; }

        public int ConsumptionUser { get; set; }

        public long Cost { get; set; }

        public long Income { get; set; }

        public long SubscriptionIncome { get; set; }

        public long Note3Price { get; set; }

        public long Note3Income { get; set; }

        public long SeasonalIncome { get; set; }

        public long TIncome { get; set; }

        public long Note7Price { get; set; }

        public long Note7Income { get; set; }
    }

    public class UpdateIncomeCurrentWsHDTO : CreateIncomeCurrentWsHDTO
    {
        public int Id { get; set; }
    }

    public class IncomeCurrentWsHDTO
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

        public int UnitUser { get; set; }

        public decimal AvgConsumeUser { get; set; }

        public int ConsumptionUser { get; set; }

        public long Cost { get; set; }

        public long Income { get; set; }

        public long SubscriptionIncome { get; set; }

        public long Note3Price { get; set; }

        public long Note3Income { get; set; }

        public long SeasonalIncome { get; set; }

        public long TIncome { get; set; }

        public long Note7Price { get; set; }

        public long Note7Income { get; set; }
    }
}
