using Ganss.Excel;

namespace Datiss.Budget.Services.Excel.Models
{
    public class CostCurrentEPaymentImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int BillingCycle { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public decimal EPayForcast { get; set; }
        
        [Column(MappingDirections.Both, Letter = "E")]
        public long EPayBFee { get; set; }
        
        [Column(MappingDirections.Both, Letter = "F")]
        public decimal PPayForcast { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long PPayBFee { get; set; }
    }
}
