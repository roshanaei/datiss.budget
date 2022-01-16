using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeCurrentOperationalDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ICOTypeId { get; set; }

        public string ICOTypeTitle { get; set; }

        public int CountH { get; set; }

        public int PriceH { get; set; }

        public int CostH { get; set; }

        public int CountNH { get; set; }

        public int PriceNH { get; set; }

        public int CostNH { get; set; }

        public int TotalCount { get; set; }

        public int TotalCost { get; set; }
    }

    public class UpdateIncomeCurrentOperationalDTO : CreateIncomeCurrentOperationalDTO
    {
        public int Id { get; set; }
    }

    public class IncomeCurrentOperationalDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ICOTypeId { get; set; }

        public string ICOTypeTitle { get; set; }

        public int CountH { get; set; }

        public int PriceH { get; set; }

        public int CostH { get; set; }

        public int CountNH { get; set; }

        public int PriceNH { get; set; }

        public int CostNH { get; set; }

        public int TotalCount { get; set; }

        public int TotalCost { get; set; }

    }
}
