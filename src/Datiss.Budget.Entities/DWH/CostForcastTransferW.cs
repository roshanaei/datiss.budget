using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.DWH
{
    public class CostForcastTransferW : IAuditableEntity
    {
        public CostForcastTransferW() { }

        #region Properties
        public int Id { get; set; }

        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int TransferTypeId { get; set; }

        public int CreditTypeId { get; set; }

        public int DigTypeId { get; set; }

        public int TubeTypeId { get; set; }

        public int DiameterPipeTypeId { get; set; }

        public int Lenth { get; set; }

        public long PipeCost { get; set; }

        public long RunCost { get; set; }

        public long  TotalCost {get;set;}

        public int ExtensionTypeId { get; set; }

        public int SuggestedBudgetTopicTypeId { get; set; }

        #endregion

        #region navigations
        public FinanceYear FinanceYear { get; set; }

        public Organization Organization { get; set; }

        public Constant TransferType { get; set; }

        public Constant Credit { get; set; }

        public Constant DigType { get; set; }

        public Constant TubeType { get; set; }

        public Constant DiameterType { get; set; }

        public Constant Extension { get; set; }

        public Constant SuggestedBudgetTopic { get; set; }
        #endregion
    }
}
