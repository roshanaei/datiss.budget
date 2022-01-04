using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateNHCoDTO
    {
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public ActivityType ActivityType { get; set; }
        public int P1Capacity { get; set; }
        public int FixCostCo { get; set; }
        public int P1CostCo { get; set; }
        public int P2CostCo { get; set; }
    }

    public class UpdateNHCoDTO : CreateNHCoDTO
    {
        public int Id { get; set; }
    }

    public class NHCoDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public ActivityType ActivityType { get; set; }
        public int P1Capacity { get; set; }
        public int FixCostCo { get; set; }
        public int P1CostCo { get; set; }
        public int P2CostCo { get; set; }
    }
}
