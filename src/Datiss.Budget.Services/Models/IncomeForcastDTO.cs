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

        public int WaterInstllIncome { get; set; }

        public int WaterBranchIncome { get; set; }

        public int WaterNote2Income { get; set; }

        public int WaterNote3Income { get; set; }

        public int WNote11Income { get; set; }
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
        public int WaterInstllIncome { get; set; }
        public int WaterBranchIncome { get; set; }
        public int WaterNote2Income { get; set; }
        public int WaterNote3Income { get; set; }
        public int WNote11Income { get; set; }
    }
}
