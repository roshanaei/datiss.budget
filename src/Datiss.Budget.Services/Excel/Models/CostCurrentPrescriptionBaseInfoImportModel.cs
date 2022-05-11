using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostCurrentPrescriptionBaseInfoImportModel
    {

        [Column(MappingDirections.Both, Letter = "A")]
        public long FixSalary { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public long HouseRt { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int Copun { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public long EmployRight { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public long StuffRt { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public long ChildRt { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long HardWorkingRt { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public long RegionRight { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public long Healths { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public long NewFixSalary { get; set; }
    }
}
