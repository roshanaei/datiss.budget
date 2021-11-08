namespace Datiss.Budget.Services.Models
{
    public class CreateWaterInstallFeeDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int DWaterTypeId { get; set; }

        public int WInstllFee { get; set; }

        public string DWaterTypeTitle { get; set; }
    }

    public class UpdateWaterInstallFeeDTO : CreateWaterInstallFeeDTO
    {
        public int Id { get; set; }

    }

    public class WaterInstallFeeDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int DWaterTypeId { get; set; }
        public string DWaterTypeDisplay { get; set; }
        public int WInstallFee { get; set; }
    }
}
