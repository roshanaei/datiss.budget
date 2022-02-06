using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{

    public class CalculationInputViewModel
    {
        public int OrganizationId { get; set; }
        public int YearId { get; set; }
        public int? UserTypeId { get; set; }
    }

    public  class CalculationResultViewModel
    {
        public string Title { get; set; }
        public int Result { get; set; }
        public decimal? DecimalResult { get; set; }
        public string ResultDisplay => Result.ToString("N0");
        public string DecimalResultDisplay => DecimalResult?.ToString("N2");

    }
}
