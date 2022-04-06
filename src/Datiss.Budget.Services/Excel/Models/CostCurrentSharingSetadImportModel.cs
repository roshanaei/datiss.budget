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
        public int WUnit { get; set; }

        [Column(3, MappingDirections.Both, Letter = "D")]
        public long IncomeCurrentW { get; set; }

        [Column(4, MappingDirections.Both, Letter = "E")]
        public decimal IncomeCurrentWSharingCoff { get; set; }

        [Column(5, MappingDirections.Both, Letter = "F")]
        public decimal SPSHahrdari { get; set; }

        [Column(6, MappingDirections.Both, Letter = "G")]
        public long IncomeForcast { get; set; }

        [Column(7, MappingDirections.Both, Letter = "H")]
        public decimal IncomeForcastsharing { get; set; }

        [Column(8, MappingDirections.Both, Letter = "I")]
        public int WsUnit { get; set; }

        [Column(9, MappingDirections.Both, Letter = "J")]
        public long IncomeCurrentWs { get; set; }

        [Column(10, MappingDirections.Both, Letter = "K")]
        public decimal IncomeCurrentWsSharingCoff { get; set; }


    }
}
