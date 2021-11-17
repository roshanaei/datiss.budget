using System;

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

    public class UpdateWasteSalesSplitDTO : CreateWasteSalesSplitDTO
    {
        public int Id { get; set; }
    }

    public class WasteSalesSplitDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public int WsPipeDiameterId { get; set; }

        public string WspipeDiameterDisplay { get; set; }

        public int NumberSales { get; set; }

        public int UnitSales { get; set; }
        public decimal AverageCapacity { get; set; }
    }
}
