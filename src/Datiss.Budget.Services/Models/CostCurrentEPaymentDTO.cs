namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentEPaymentDTO
    {
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public int BillingCycle { get; set; }
        public decimal EPayForcast { get; set; }
        public long EPayBFee { get; set; }
        public decimal PPayForcast { get; set; }
        public long PPayBFee { get; set; }
    }

    public class UpdateCostCurrentEPaymentDTO : CreateCostCurrentEPaymentDTO
    {
        public int Id { get; set; }
    }
    public class CostCurrentEPaymentDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int BillingCycle { get; set; }

        public decimal EPayForcast { get; set; }

        public long EPayBFee { get; set; }

        public decimal PPayForcast { get; set; }

        public long PPayBFee { get; set; }
    }
}
