using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class SubscriptionImportModel
    {
        [Column(0, MappingDirections.Both, Letter = "A")]
        public int YearId { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int UserTypeId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public int SubW { get; set; }

        [Column(3, MappingDirections.Both, Letter = "D")]
        public int SubWs { get; set; }
    }
}
