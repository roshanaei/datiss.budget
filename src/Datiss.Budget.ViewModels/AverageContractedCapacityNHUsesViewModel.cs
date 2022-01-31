using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class AverageContractedCapacityNHUsesViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public decimal AverageCapacity { get; set; }

        public string AverageCapacityDisplay => AverageCapacity.ToString("N2");

        public decimal AverageCapacityWs { get; set; }

        public string AverageCapacityWsDisplay => AverageCapacityWs.ToString("N2");

        public decimal AverageCapacityIncome { get; set; }

        public string AverageCapacityIncomeDisplay => AverageCapacityIncome.ToString("N2");

        public decimal AverageCapacityWsIncome { get; set; }

        public string AverageCapacityWsIncomeDisplay => AverageCapacityWsIncome.ToString("N2");
    }
}
