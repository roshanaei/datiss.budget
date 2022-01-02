using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class WaterInstallFeeImportModel
    {

        [Column(0, MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public string DWaterTypeDisplay { get; set; }

        [Column(3, MappingDirections.Both, Letter = "D")]
        public int DWaterTypeId { get; set; }

        [Column(4, MappingDirections.Both, Letter = "E")]
        public int WInstallFee { get; set; }

    }
}
