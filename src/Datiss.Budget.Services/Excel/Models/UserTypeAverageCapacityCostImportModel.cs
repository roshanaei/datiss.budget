using Ganss.Excel;

namespace Datiss.Budget.Services.Excel.Models
{
    public class UserTypeAverageCapacityCostImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string UserTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int UserTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public decimal AverageCapacityWIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public decimal AverageCapacityWsIncome { get; set; }
    }
}
