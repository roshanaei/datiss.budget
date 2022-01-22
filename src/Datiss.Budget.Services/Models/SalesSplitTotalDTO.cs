using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateSalesSplitTotalDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public int WNumber { get; set; }

        public int WUnit { get; set; }

        public int WsNumber { get; set; }

        public int WsUnit { get; set; }

        public int WNumber_2 { get; set; }

        public int WUnit_2 { get; set; }

        public int WsNumber_2 { get; set; }

        public int WsUnit_2 { get; set; }
    }

    public class UpdateSalesSplitTotalDTO : CreateSalesSplitTotalDTO
    {
        public int Id { get; set; }
    }

    public class SalesSplitTotalDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int WNumber { get; set; }
        public int WUnit { get; set; }
        public int WsNumber { get; set; }
        public int WsUnit { get; set; }
        public int WNumber_2 { get; set; }
        public int WUnit_2 { get; set; }
        public int WsNumber_2 { get; set; }
        public int WsUnit_2 { get; set; }
    }
}
