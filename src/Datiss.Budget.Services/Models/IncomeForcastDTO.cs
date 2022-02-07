using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeForcastDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public int NumberUser { get; set; }

        public int UnitUser { get; set; }

        public long WaterInstllIncome { get; set; }

        public long WaterBranchIncome { get; set; }

        public long WaterNote2Income { get; set; }

        public long WaterNote3Income { get; set; }

        public long WNote11Income { get; set; }
    }

    public class UpdateIncomeForcastDTO : CreateIncomeForcastDTO
    {
        public int Id { get; set; }
    }

    public class IncomeForcastDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int NumberUser { get; set; }
        public int UnitUser { get; set; }
        public long WaterInstllIncome { get; set; }
        public long WaterBranchIncome { get; set; }
        public long WaterNote2Income { get; set; }
        public long WaterNote3Income { get; set; }
        public long WNote11Income { get; set; }
    }
}
