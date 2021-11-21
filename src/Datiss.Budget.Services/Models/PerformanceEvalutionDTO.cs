using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreatePerformanceEvaluationDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public EntityStatus Status { get; set; }

        public int TableFieldId { get; set; }

        public decimal Target { get; set; }

        public decimal Operation { get; set; }
    }

    public class UpdatePerformanceEvaluationDTO : CreatePerformanceEvaluationDTO
    {
        public int Id { get; set; }

    }

    public class PerformanceEvaluationDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public EntityStatus Status { get; set; }
        public int TableFieldId { get; set; }
        public string TableFieldDisplay { get; set; }
        public decimal Target { get; set; }
        public decimal Operation { get; set; }
    }
}
