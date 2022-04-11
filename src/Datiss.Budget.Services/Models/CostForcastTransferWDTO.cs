namespace Datiss.Budget.Services.Models
{
    public class CreateCostForcastTransferWDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int TransferTypeId { get; set; }

        public int CreaditTypeId { get; set; }

        public int DigTypeId { get; set; }

        public int TubeTypeId { get; set; }

        public int DiameterPipeTypeId { get; set; }

        public int Lenth { get; set; }

        public long PipeCost { get; set; }

        public long RunCost { get; set; }

        public long TotalCost { get; set; }

        public int ExtensionTypeId { get; set; }

        public int SuggestedBudgetTopicTypeId { get; set; }

    }

    public class UpdateCostForcastTransferWDTO : CreateCostForcastTransferWDTO
    {
        public int Id { get; set; }

    }

    public class CostForcastTransferWDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int TransferTypeId { get; set; }
        public string TransferTypeDisplay { get; set; }

        public int CreaditTypeId { get; set; }
        public string CreaditTypeDisplay { get; set; }

        public int DigTypeId { get; set; }
        public string DigTypeDisplay { get; set; }

        public int TubeTypeId { get; set; }
        public string TubeTypeDisplay { get; set; }

        public int DiameterPipeTypeId { get; set; }
        public string DiameterPipeTypeDisplay { get; set; }

        public int Lenth { get; set; }

        public long PipeCost { get; set; }

        public long RunCost { get; set; }

        public long TotalCost { get; set; }

        public int ExtensionTypeId { get; set; }
        public string ExtensionTypeDisplay { get; set; }

        public int SuggestedBudgetTopicTypeId { get; set; }
        public string SuggestedBudgetTopicTypeDisplay { get; set; }
    }
}
