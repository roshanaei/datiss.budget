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
        public string TableFieldTitle { get; set; }
        public decimal Target { get; set; }
        public decimal Operation { get; set; }
        public int Month { get; set; }
        public decimal PercentRealization { get; set; }
        public decimal Budget { get; set; }
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
        public bool Status { get; set; }
        public int TableFieldId { get; set; }
        public string TableFieldDisplay { get; set; }
        public decimal Target { get; set; }
        public decimal Operation { get; set; }
        public int Month { get; set; }
        public decimal PercentRealization
        {
            get
            {
                if (Budget == 0 || Operation == 0)
                    return 0;
                return (Budget/Operation)*100;
            }
        }
        public decimal Budget
        {
            get
            {
                if (Month == 0 || Target == 0)
                    return 0;
                return (Target / 12) * Month;
            }
        }
    }
}
