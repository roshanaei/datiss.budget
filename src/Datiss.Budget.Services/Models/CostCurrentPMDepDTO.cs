using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentPMDepDTO
    {

    }

    public class UpdateCostCurrentPMDepDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int CCPMDepTypeId { get; set; }

        public string CCPMDepTypeTitle { get; set; }

        public int CostCenterTypeId { get; set; }

        public string CostCenterTypeTitle { get; set; }
        
        public ActivityType ActivityType { get; set; }

        public RecordType RecordType { get; set; }

        public long FinancePMCost { get; set; }

        public decimal RFinancePMCost_D { get; set; }

        public long FinanceDepCost { get; set; }

        public decimal RFinanceDepCost_D { get; set; }
    }

    public class CostCurrentPMDepDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int CCPMDepTypeId { get; set; }
        public string CCPMDepTypeDisplay { get; set; }

        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public RecordType RecordType { get; set; }

        public long FinancePMCost { get; set; }

        public decimal RFinancePMCost_D { get; set; }

        public long FinanceDepCost { get; set; }

        public decimal RFinanceDepCost_D { get; set; }
    }
}
