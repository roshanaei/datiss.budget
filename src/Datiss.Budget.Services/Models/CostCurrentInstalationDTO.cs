using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentInstalationDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int CCInstalationTypeId { get; set; }

        public string CCInstalationTypeTitle { get; set; }

        public int NumberUser { get; set; }

        public int Cost { get; set; }

        public long Income { get; set; }
    }

    public class UpdateCostCurrentInstalationDTO : CreateCostCurrentInstalationDTO
    {
        public int Id { get; set; }
    }

    public class CostCurrentInstalationDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay { get; set; }

        public int CCInstalationTypeId { get; set; }

        public string CCInstalationTypeTitle { get; set; }

        public int NumberUser { get; set; }

        public int Cost { get; set; }

        public long Income { get; set; }
    }

}
