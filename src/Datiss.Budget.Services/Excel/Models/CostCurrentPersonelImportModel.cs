using Ganss.Excel;

namespace Datiss.Budget.Services.Excel.Models
{
    public class CostCurrentPersonelImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int PersonelCode { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public string FirstName { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public string LastName { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public bool GenderId { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int GradeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int ContractTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int JobDepartmentTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int JobStatusTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int JobStatusDetailTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int ExperienceYear { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int ExperienceMonth { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long FixSalary { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long EmployRight { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long RegionRight { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int OverTimeValue { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long OverTimeCost { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public int HolidayValue { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long HolidayCost { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long ShiftPercent { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long ShiftPCost { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long MissionCount { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long MissionDayCost { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long HardWorkingRt { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long TrafficRt { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long HouseRt { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long ChildRt { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long StuffRt { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long Education { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long InsuranceMaster { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long InsuranceAging { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long HolidayYearly { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long MilitaryServiceCost { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long UnUseHolidayCount { get; set; }

        [Column(MappingDirections.Both, Letter = "A")]
        public long WelfareCost { get; set; }
    }
}
