using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class WasteSalesSplitImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string UserTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int UserTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string WspipeDiameterDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int WsPipeDiameterId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int NumberSales { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int UnitSales { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public decimal AverageCapacity { get; set; }
    }
}
