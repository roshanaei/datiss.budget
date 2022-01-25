using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateCofficientDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CofficientTypeId { get; set; }

        public string CofficientTypeTitle { get; set; }

        public CofficientsGroup GroupName { get; set; }

        public decimal Fee { get; set; }
    }

    public class UpdateCofficientDTO : CreateCofficientDTO
    {
        public int Id { get; set; }
    }

    public class CofficientDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CofficientTypeId { get; set; }
        public string CofficientTypeTitle { get; set; }
        public CofficientsGroup GroupName { get; set; }
        public decimal Fee { get; set; }
    }
}
