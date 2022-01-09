using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class WaterInstallFeeImportModel
    {

        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string DWaterTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int DWaterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int WInstallFee { get; set; }

    }
}
