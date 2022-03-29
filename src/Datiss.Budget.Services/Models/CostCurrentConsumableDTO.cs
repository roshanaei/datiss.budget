using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Models
{

    public class CreateCostCurrentConsumableDTO
    {
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public ActivityType ActivityType { get; set; }
        public int ConsumableTypeId { get; set; }
        public string ConsumableTypeDisplay { get; set; }
        public int ConsumableAmount { get; set; }
        public long ConsumableCost { get; set; }
    }

    public class UpdateCostCurrentConsumableDTO : CreateCostCurrentConsumableDTO
    {
        public int Id { get; set; }
    }

    public class CostCurrentConsumableDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public ActivityType ActivityType { get; set; }
        public int ConsumableTypeId { get; set; }
        public string ConsumableTypeDisplay { get; set; }
        public int ConsumableAmount { get; set; }
        public long ConsumableCost { get; set; }
    }
}
