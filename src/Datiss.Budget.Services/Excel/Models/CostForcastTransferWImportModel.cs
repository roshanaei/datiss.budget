using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostForcastTransferWImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public int TransferTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public string Location { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int CreditTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int DigTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int TubeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int DiameterPipeTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int Lenth { get; set; }

        //[Column(MappingDirections.Both, Letter = "J")]
        //public long PipeCost { get; set; }

        //[Column(MappingDirections.Both, Letter = "k")]
        //public long RunCost { get; set; }

        //[Column(MappingDirections.Both, Letter = "L")]
        //public long TotalCost { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public int ExtensionTypeId { get; set; }

    }
}
