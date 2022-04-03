using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentNOViewModelViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int CostCurrentNoTypeId { get; set; }


        [Required(ErrorMessage = "*")]
        public long BaseFee { get; set; }
        
        [Required(ErrorMessage = "*")]
        public long LastYearFee { get; set; }


        public IEnumerable<SelectListItem> CostCurrentNOTypeSource { get; set; }

        public string CostCurrentNOTypeTitle
        {
            get
            {
                if(CostCurrentNOTypeSource == null || !CostCurrentNOTypeSource.Any())
                    return string.Empty;

                return CostCurrentNOTypeSource.FirstOrDefault(x => x.Value.ToString() == CostCurrentNoTypeId.ToString()).Text;
            }
        }

    }

    public class UpdateCostCurrentNOViewModel : CreateCostCurrentNOViewModelViewModel
    {
        public int Id { get; set; }

        public long ForcastFee { get; set; }
    }

    public class CostCurrentNOViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int CostCurrentNOTypeId { get; set; }

        public string CostCurrentNOTypeTitle { get; set; }
        
        public long BaseFee { get; set; }

        public string BaseFeeDisplay => BaseFee.ToString("N0");

        public long LastYearFee { get; set; }

        public string LastYearFeeDisplay => LastYearFee.ToString("N0");

        public long ForcastFee { get; set; }

        public string ForcastFeeDisplay => ForcastFee.ToString("N0");

    }

    public class CostCurrentNOFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set;}

        public int? CostCurrentNOTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }
}
