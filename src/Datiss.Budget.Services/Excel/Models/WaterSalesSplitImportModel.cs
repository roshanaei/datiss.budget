using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class WaterSalesSplitImportModel
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
        public string WPipeDiameterDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int WPipeDiameterId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int NumberSales { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int UnitSales { get; set; }

        //[Column(MappingDirections.Both, Letter = "I")]
        //public decimal AverageCapacity { get; set; }

    }
}
