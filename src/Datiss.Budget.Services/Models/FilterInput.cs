using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public abstract class FilterInput
    {
        public string Search { get; set; }
        public string OrderBy { get; set; }
        public bool OrderDesc { get; set; }
    }

    public enum WaterInstallFeeFilterMode
    {
        Exact = 0,
        LessThan = 1,
        GreaterThan = 2
    }

    public class WaterInstallFeeFilter: FilterInput
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? DWaterTypeId { get; set; }
        public int? WInstallFee { get; set; }
        public WaterInstallFeeFilterMode FeeMode { get; set; }
    }
}
