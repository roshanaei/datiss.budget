using Ganss.Excel;

namespace Datiss.Budget.Services.Excel.Models
{

    public class CostCurrentSharingSetadImportModel
    {

        [Column(0, MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public decimal IncomeCurrentWSharingCoff { get; set; }

        [Column(3, MappingDirections.Both, Letter = "D")]
        public decimal IncomeCurrentWsSharingCoff { get; set; }

        [Column(4, MappingDirections.Both, Letter = "E")]
        public decimal IncomeForcastsharing { get; set; }
    }
}
