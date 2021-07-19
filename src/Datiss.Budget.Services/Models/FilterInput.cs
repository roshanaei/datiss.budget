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
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public int StartIndex => (PageNumber * PageSize) - PageSize;
    }

    public enum InstallFeeFilterMode
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
        public InstallFeeFilterMode FeeMode { get; set; }
    }

    public class WasteInstallFeeFilter : FilterInput
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? DWasteTypeId { get; set; }
        public int? WInstallFee { get; set; }
        public InstallFeeFilterMode FeeMode { get; set; }
    }

    public class WaterSalesSplitFilter : FilterInput
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public int? WPipeDiameterId { get; set; }
        public int? NumberSales { get; set; }
        public int? UnitSales { get; set; }
        public InstallFeeFilterMode NumberMode { get; set; }
        public InstallFeeFilterMode UnitMode { get; set; }
    }
}
