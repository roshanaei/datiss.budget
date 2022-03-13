namespace Datiss.Budget.Services.Models
{
    public class CreateUserTypeAverageCapacityCostDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public decimal AverageCapacityWIncome { get; set; }

        public decimal AverageCapacityWsIncome { get; set; }
    }

    public class UpdateUserTypeAverageCapacityCostDTO : CreateUserTypeAverageCapacityCostDTO
    {
        public int Id { get; set; }
    }

    public class UserTypeAverageCapacityCostDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public decimal AverageCapacityWIncome { get; set; }
        public decimal AverageCapacityWsIncome { get; set; }
    }
}
