using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateUserTypeAverageCapacityForcastDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public decimal AverageCapacityW { get; set; }

        public decimal AverageCapacityWs { get; set; }

        public decimal AverageCapacityWIncome { get; set; }

        public decimal AverageCapacityWsIncome { get; set; }
    }

    public class UpdateUserTypeAverageCapacityForcastDTO : CreateUserTypeAverageCapacityForcastDTO
    {
        public int Id { get; set; }
    }

    public class UserTypeAverageCapacityForcastDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public decimal AverageCapacityW { get; set; }
        public decimal AverageCapacityWs { get; set; }
        public decimal AverageCapacityWIncome { get; set; }
        public decimal AverageCapacityWsIncome { get; set; }
    }
}
