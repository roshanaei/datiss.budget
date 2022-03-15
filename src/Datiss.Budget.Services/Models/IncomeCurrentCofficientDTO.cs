namespace Datiss.Budget.Services.Models
{
    public class CreateIncomeCurrentCofficientDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public int UsageLayerId { get; set; }

        public string UsageLayerTitle { get; set; }

        public decimal Fee { get; set; }

    }

    public class UpdateIncomeCurrentCofficientDTO : CreateIncomeCurrentCofficientDTO
    {
        public int Id { get; set; }
    }

    public class IncomeCurrentCofficientDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public int UsageLayerId { get; set; }

        public string UsageLayerTitle { get; set; }

        public decimal Fee { get; set; }

    }
}