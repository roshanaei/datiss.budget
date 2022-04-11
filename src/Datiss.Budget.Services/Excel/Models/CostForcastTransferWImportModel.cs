using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostForcastTransferWImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int TransferTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string Location { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int CreditTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int DigTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int TubeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int DiameterPipeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public int Lenth { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public long PipeCost { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public long RunCost { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public long TotalCost { get; set; }

        [Column(MappingDirections.Both, Letter = "N")]
        public int ExtensionTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "O")]
        public int SuggestedBudgetTopicTypeId { get; set; }

    }
}
