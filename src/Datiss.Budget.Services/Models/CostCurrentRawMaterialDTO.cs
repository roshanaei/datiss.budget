using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Models
{
    public class CreateCostCurrentRawMaterialDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public ActivityType ActivityType { get; set; }

        public int RawMaterialTypeId { get; set; }

        public string RawMaterialTypeDisplay { get; set; }

        public long BaseFee { get; set; }

        public long LastYearFee { get; set; }

    }

    public class UpdateCostCurrentRawMaterialDTO : CreateCostCurrentRawMaterialDTO
    {
        public int Id { get; set; }
        public long ForcastFee { get; set; }

    }

    public class CostCurrentRawMaterialDTO
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay { get; set; }

        public int RawMaterialTypeId { get; set; }

        public string RawMaterialTypeDisplay { get; set; }

        public long BaseFee { get; set; }

        public long LastYearFee { get; set; }

        public long ForcastFee { get; set; }
    }

}
