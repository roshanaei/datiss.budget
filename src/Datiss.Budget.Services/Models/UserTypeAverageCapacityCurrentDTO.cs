namespace Datiss.Budget.Services.Models
{
    public class CreateUserTypeAverageCapacityCurrentDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public decimal AverageCapacityWIncome { get; set; }

        public decimal AverageCapacityWsIncome { get; set; }

        public decimal SummerIndex { get; set; }

    }

    public class UpdateUserTypeAverageCapacityCurrentDTO : CreateUserTypeAverageCapacityCurrentDTO
    {
        public int Id { get; set; }
    }

    public class UserTypeAverageCapacityCurrentDTO
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
        public decimal SummerIndex { get; set; }

    }
}
