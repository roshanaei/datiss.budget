using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeCurrentWNHDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int NumberUser { get; set; }

        public int UnitUser { get; set; }

        public decimal AvgConsumeUser { get; set; }

        public decimal ExcessConsumption { get; set; }

        public int ConsumptionUser { get; set; }

        public decimal Capacity { get; set; }

        public long Cost { get; set; }

        public long Income { get; set; }

        public long ExcessIncome { get; set; }

        public long SeasonalIncome { get; set; }

        public long Note3Price { get; set; }

        public long Note3Income { get; set; }

        public long SubscriptionIncome { get; set; }

        public long TotalIncome { get; set; }

        public int Diff_ConsWsVolume { get; set; }

        public long Note2Income { get; set; }

        public int WasteVolume { get; set; }
    }
    public class UpdateIncomeCurrentWNHDTO : CreateIncomeCurrentWNHDTO
    {
        public int Id { get; set; }
    }
    public class IncomeCurrentWNHDTO
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
        public decimal ExcessConsumption { get; set; }
        public int ConsumptionUser { get; set; }
        public decimal Capacity { get; set; }
        public long Cost { get; set; }
        public long Income { get; set; }
        public long ExcessIncome { get; set; }
        public long SeasonalIncome { get; set; }
        public long Note3Price { get; set; }
        public long Note3Income { get; set; }
        public long SubscriptionIncome { get; set; }
        public long TotalIncome { get; set; }
        public int Diff_ConsWsVolume { get; set; }
        public long Note2Income { get; set; }
        public int WasteVolume { get; set; }
    }
}
