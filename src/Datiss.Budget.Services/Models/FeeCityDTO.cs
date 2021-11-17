using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateFeeCityDTO
    {
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public decimal DomesticPrice { get; set; }
        public decimal NDomesticPrice { get; set; }
    }

    public class UpdateFeeCityDTO : CreateFeeCityDTO
    {
        public int Id { get; set; }
    }

    public class FeeCityDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public decimal DomesticPrice { get; set; }
        public decimal NDomesticPrice { get; set; }
    }
}
