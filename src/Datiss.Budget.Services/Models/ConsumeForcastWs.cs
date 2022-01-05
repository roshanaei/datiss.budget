namespace Datiss.Budget.Services.Models
{
    public class CreateConsumeForcastWsDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public int UsageLayerId { get; set; }

        public decimal CountUser { get; set; }

        public decimal UnitUser { get; set; }

        public decimal ConsumeUser { get; set; }

        public decimal AvgConsumeUser { get; set; }
    }

    public class UpdateConsumeForcastWsDTO : CreateConsumeForcastWsDTO
    {
        public int Id { get; set; }

        public decimal ConsumeUserForcast { get; set; }
    }

    public class ConsumeForcastWsDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }

        public int UsageLayerId { get; set; }

        public decimal CountUser { get; set; }

        public decimal UnitUser { get; set; }

        public decimal ConsumeUser { get; set; }

        public decimal AvgConsumeUser { get; set; }

        public decimal ConsumeUserForcast { get; set; }
    }
}
