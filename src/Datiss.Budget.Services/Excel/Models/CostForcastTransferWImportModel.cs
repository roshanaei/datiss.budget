using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostForcastTransferWImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int TransferTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string TransferTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int CreaditTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string CreaditTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int DigTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string DigTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int TubeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string TubeTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int DiameterPipeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string DiameterPipeTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int Lenth { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public long PipeCost { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public long RunCost { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public long TotalCost { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int ExtensionTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string ExtensionTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int SuggestedBudgetTopicTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string SuggestedBudgetTopicTypeDisplay { get; set; }

    }
}
