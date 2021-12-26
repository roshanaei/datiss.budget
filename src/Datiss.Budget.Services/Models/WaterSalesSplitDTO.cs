using System;
using System.Collections.Generic;

namespace Datiss.Budget.Services.Models
{
    public class CreateWaterSalesSplitDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public int WPipeDiameterId { get; set; }

        public string WPipeDiameterTitle { get; set; }

        public int NumberSales { get; set; }

        public int UnitSales { get; set; }

        public decimal AverageCapacity { get; set; }

    }

    public class UpdateWaterSalesSplitDTO : CreateWaterSalesSplitDTO
    {
        public int Id { get; set; }
    }

    public class WaterSalesSplitDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int WPipeDiameterId { get; set; }
        public string WPipeDiameterDisplay { get; set; }
        public int NumberSales { get; set; }
        public int UnitSales { get; set; }
        public decimal AverageCapacity { get; set; }
    }

}
