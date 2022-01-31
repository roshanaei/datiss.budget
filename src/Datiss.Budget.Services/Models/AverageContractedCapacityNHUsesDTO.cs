using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateAverageContractedCapacityNHUsesDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public decimal AverageCapacity { get; set; }

        public decimal AverageCapacityWs { get; set; }

        public decimal AverageCapacityIncome { get; set; }

        public decimal AverageCapacityWsIncome { get; set; }
        
        public string UserTypeTitle { get; set; }
    }

    public class UpdateAverageContractedCapacityNHUsesDTO : CreateAverageContractedCapacityNHUsesDTO
    {
        public int Id { get; set; }
    }

    public class AverageContractedCapacityNHUsesDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public decimal AverageCapacity { get; set; }

        public decimal AverageCapacityWs { get; set; }

        public decimal AverageCapacityIncome { get; set; }

        public decimal AverageCapacityWsIncome { get; set; }
    }
}
