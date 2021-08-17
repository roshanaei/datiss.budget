using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Entities.DWH
{
    public  class WasteSalesSplit
    {
        public WasteSalesSplit() { }

        #region Properties
         
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set;}

        public int UserTypeId { get; set; }

        public int WsPipeDiameterId { get; set; }

        public int NumberSales { get; set; }

        public int UnitSales { get; set; }
        #endregion

        #region Navigation
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant UserType { get; set; }

        public Constant WsPipeDiameter { get; set; }
        #endregion
    }
}
