using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentElectricityDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ElectricityAmount { get; set; }

        public long ElectricityCost { get; set; }
    }

    public class UpdateCostCurrentElectricityDTO : CreateCostCurrentElectricityDTO
    {
        public int Id { get; set; }
    }

    public class CostCurrentElectricityDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ElectricityAmount { get; set; }

        public long ElectricityCost { get; set; }

    }
}
