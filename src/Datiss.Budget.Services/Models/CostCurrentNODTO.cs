using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public  class CreateCostCurrentNODTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CostCurrentNoTypeId { get; set; }

        public string CostCurrentNoTypeTitle { get; set; }

        public long BaseFee { get; set; }

        public long LastYearFee { get; set; }
    }

    public class UpdateCostCurrentNODTO :CreateCostCurrentNODTO
    {
        public int Id { get; set; }

        public long ForcastFee { get; set; }

    }

    public class CostCurrentNODTO
    {
        public int Id { get; set; }

        public int YearId { get; set; } 

        public int Year { get; set; }

        public int OrganizationId { get; set; } 

        public string OrganizationDisplay { get; set; }

        public int CostCurrentNoTypeId { get; set; }

        public string CostCurrentNoTypeDisplay { get; set; }

        public long BaseFee { get; set; }

        public long LastYearFee { get; set; }

        public long ForcastFee { get; set; }

    }
}
