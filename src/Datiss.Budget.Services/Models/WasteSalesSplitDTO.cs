using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateWasteSalesSplitDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public String UserTypeTitle { get; set; }

        public int WsPipeDiameterId { get; set; }

        public string WsPipeDiameterTitle { get; set; }

        public int NumberSales { get; set; }

        public int UnitSales { get; set; }

    }
}
