using Ganss.Excel;

namespace Datiss.Budget.Services.Excel.Models
{
    public class CostCurrentPersonelImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string FirstName { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public string LastName { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int GenderVal { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int PersonelCode { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int ContractTypeId { get; set; }
        
        [Column(MappingDirections.Both, Letter = "H")]
        public long FixSalary { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public long EmployRight { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public long RegionRight { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public int GradeTypeId { get; set; }

        //[Column(MappingDirections.Both, Letter = "L")]
        //public int JobDepartmentTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public int JobStatusTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public int JobStatusDetailTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "N")]
        public int ExperienceYear { get; set; }

        [Column(MappingDirections.Both, Letter = "O")]
        public int ExperienceMonth { get; set; }

        [Column(MappingDirections.Both, Letter = "P")]
        public int OverTimeValue { get; set; }

        [Column(MappingDirections.Both, Letter = "Q")]
        public long OverTimeCost { get; set; }

        [Column(MappingDirections.Both, Letter = "R")]
        public int HolidayValue { get; set; }

        [Column(MappingDirections.Both, Letter = "S")]
        public long HolidayCost { get; set; }

        [Column(MappingDirections.Both, Letter = "T")]
        public long ShiftPercent { get; set; }

        [Column(MappingDirections.Both, Letter = "U")]
        public long ShiftPCost { get; set; }

        [Column(MappingDirections.Both, Letter = "V")]
        public long MissionCount { get; set; }

        [Column(MappingDirections.Both, Letter = "W")]
        public long MissionDayCost { get; set; }

        [Column(MappingDirections.Both, Letter = "X")]
        public long HardWorkingRt { get; set; }

        [Column(MappingDirections.Both, Letter = "Y")]
        public long TrafficRt { get; set; }

        [Column(MappingDirections.Both, Letter = "Z")]
        public long HouseRt { get; set; }

        [Column(MappingDirections.Both, Letter = "AA")]
        public long ChildRt { get; set; }

        [Column(MappingDirections.Both, Letter = "AB")]
        public long StuffRt { get; set; }

        [Column(MappingDirections.Both, Letter = "AC")]
        public long Education { get; set; }

        [Column(MappingDirections.Both, Letter = "AD")]
        public long InsuranceMaster { get; set; }

        [Column(MappingDirections.Both, Letter = "AE")]
        public long InsuranceAging { get; set; }

        [Column(MappingDirections.Both, Letter = "AF")]
        public long HolidayYearly { get; set; }

        [Column(MappingDirections.Both, Letter = "AG")]
        public long MilitaryServiceCost { get; set; }

        [Column(MappingDirections.Both, Letter = "AH")]
        public long EndJobReward { get; set; }

        [Column(MappingDirections.Both, Letter = "AI")]
        public long WelfareCost { get; set; }

        [Column(MappingDirections.Both, Letter = "AJ")]
        public long RetirementMonth { get; set; }

        
    }
}
