using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeForcastWsDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public int NumberUser { get; set; }

        public int UnitUser { get; set; }

        public int WasteInstllIncome { get; set; }

        public int WasteBranchIncome { get; set; }

        public int WasteNote2Income { get; set; }

        public int WasteNote3Income { get; set; }

        public int WsNote11Income { get; set; }
    }

    public class UpdateIncomeForcastWsDTO : CreateIncomeForcastWsDTO
    {
        public int Id { get; set; }
    }

    public class IncomeForcastWsDTO
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
        public int WasteInstllIncome { get; set; }
        public int WasteBranchIncome { get; set; }
        public int WasteNote2Income { get; set; }
        public int WasteNote3Income { get; set; }
        public int WsNote11Income { get; set; }
    }
}
