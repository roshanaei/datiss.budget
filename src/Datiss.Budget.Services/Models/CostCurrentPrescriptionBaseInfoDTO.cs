namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentPrescriptionBaseInfoDTO
    {
        public int YearId { get; set; }

        public long FixSalary { get; set; }

        public long HouseRt { get; set; }

        public long EmployRight { get; set; }

        public long RegionRight { get; set; }

        public int Copun { get; set; }

        public long ChildRt { get; set; }

        public long StuffRt { get; set; }

        public long HardWorkingRt { get; set; }

        public long Healths { get; set; }

        public long NewFixSalary { get; set; }
    }

    public class UpdateCostCurrentPrescriptionBaseInfoDTO : CreateCostCurrentPrescriptionBaseInfoDTO
    {
        public int Id { get; set; }

    }

    public class CostCurrentPrescriptionBaseInfoDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public long FixSalary { get; set; }
        public long HouseRt { get; set; }
        public long EmployRight { get; set; }
        public long RegionRight { get; set; }
        public int Copun { get; set; }
        public long ChildRt { get; set; }
        public long StuffRt { get; set; }
        public long HardWorkingRt { get; set; }
        public long Healths { get; set; }
        public long NewFixSalary { get; set; }
    }
}
