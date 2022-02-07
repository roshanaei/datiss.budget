using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeCurrentWsNHDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int NumberUser { get; set; }

        public int UnitUser { get; set; }

        public decimal AvgConsumeUser { get; set; }

        public decimal Capacity { get; set; }

        public int ConsumptionUser { get; set; }

        public long Cost { get; set; }

        public long Income { get; set; }

        public long SubscriptionIncome { get; set; }

        public int ExcessIncome { get; set; }

        public long SeasonalIncome { get; set; }

        public long Note3Price { get; set; }

        public long Note3Income { get; set; }

        public long TotalIncome { get; set; }

        public string UserTypeTitle { get; set; }
    }

    public class UpdateIncomeCurrentWsNHDTO : CreateIncomeCurrentWsNHDTO
    {
        public int Id { get; set; }
    }

    public class IncomeCurrentWsNHDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public int NumberUser { get; set; }

        public int UnitUser { get; set; }

        public decimal AvgConsumeUser { get; set; }

        public decimal Capacity { get; set; }

        public int ConsumptionUser { get; set; }

        public long Cost { get; set; }

        public long Income { get; set; }

        public long SubscriptionIncome { get; set; }

        public int ExcessIncome { get; set; }

        public long SeasonalIncome { get; set; }

        public long Note3Price { get; set; }

        public long Note3Income { get; set; }

        public long TotalIncome { get; set; }
    }
}
