namespace Datiss.Budget.Services.Models
{
    public class CreateWasteInstallFeeDTO
    {
        public int YearId { get; set; }

        public int OrganizationId { get; set; }

        public int DWasteTypeId { get; set; }

        public int WInstllFee { get; set; }

        public string DWasteTypeTitle { get; set; }
    }

    public class UpdateWasteInstallFeeDTO : CreateWasteInstallFeeDTO
    { 
        public int Id { get; set; }
    }

    public class WasteInstallFeeDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int DWasteTypeId { get; set; }
        public string DWasteTypeDisplay { get; set; }
        public int WsInstallFee { get; set; }
    }

}
