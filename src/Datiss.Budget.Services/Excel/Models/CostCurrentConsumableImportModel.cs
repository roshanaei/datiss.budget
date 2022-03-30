using Ganss.Excel;

namespace Datiss.Budget.Services.Excel.Models
{
    public class CostCurrentConsumableImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string ConsumableTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int ConsumableTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int ConsumableAmount { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public long ConsumableCost { get; set; }
    }
}
