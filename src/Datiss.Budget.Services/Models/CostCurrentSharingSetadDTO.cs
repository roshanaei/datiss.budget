namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentSharingSetadDTO
    {
        public int YearId { get; set; }

        public int WUnit { get; set; }

        public long IncomeCurrentW { get; set; }

        public decimal IncomeCurrentWSharingCoff { get; set; }

        public int WsUnit { get; set; }

        public long IncomeCurrentWs { get; set; }

        public decimal IncomeCurrentWsSharingCoff { get; set; }

        public long IncomeForcast { get; set; }

        public decimal SPSHahrdari { get; set; }

        public decimal IncomeForcastsharing { get; set; }
    }

    public class UpdateCostCurrentSharingSetadDTO : CreateCostCurrentSharingSetadDTO
    {
        public int Id { get; set; }
    }

    public class CostCurrentSharingSetadDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int WUnit { get; set; }

        public long IncomeCurrentW { get; set; }

        public decimal IncomeCurrentWSharingCoff { get; set; }

        public int WsUnit { get; set; }

        public long IncomeCurrentWs { get; set; }

        public decimal IncomeCurrentWsSharingCoff { get; set; }

        public long IncomeForcast { get; set; }

        public decimal SPSHahrdari { get; set; }

        public decimal IncomeForcastsharing { get; set; }
    }

}
