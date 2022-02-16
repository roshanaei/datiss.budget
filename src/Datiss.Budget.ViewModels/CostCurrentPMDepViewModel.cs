using Datiss.Budget.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class UpdateCostCurrentPMDepViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int CCPMDepTypeId { get; set; }

        public ActivityType ActivityType { get; set; }
        public RecordType RecordType { get; set; }
        public long CostCenter { get; set; }
        public long FinancePMCost { get; set; }
        public decimal RFinancePMCost_D { get; set; }
        public long FinanceDepCost { get; set; }
        public decimal RFinanceDepCost_D { get; set; }
        public IEnumerable<SelectListItem> CCPMDepTypeSource { get; set; }

    }

    public class CostCurrentPMDepViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int CCPMDepTypeId { get; set; }
        public string CCPMDepTypeDisplay { get; set; }
        public ActivityType ActivityType { get; set; }
        public RecordType RecordType { get; set; }
        public long CostCenter { get; set; }
        public string CostCenterDisplay => CostCenter.ToString("N0");
        public long FinancePMCost { get; set; }
        public string FinancePMCostDisplay => FinancePMCost.ToString("N0");
        public decimal RFinancePMCost_D { get; set; }
        public string RFinancePMCost_DDisplay => RFinancePMCost_D.ToString("N2");
        public long FinanceDepCost { get; set; }
        public string FinanceDepCostDisplay => FinanceDepCost.ToString("N0");
        public decimal RFinanceDepCost_D { get; set; }
        public string RFinanceDepCost_DDisplay => RFinanceDepCost_D.ToString("N2");
    }

    public class CostCurrentPMDepFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? CCPMDepTypeId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostCurrentPMDepIndexViewModel : PagedViewModel<CostCurrentPMDepViewModel>
    {

        public CostCurrentPMDepIndexViewModel()
        {
            Filter = new CostCurrentPMDepFilterViewModel();
        }

        public CostCurrentPMDepFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> CCPMDepTypeSource { get; set; }


        public IFormFile ExcelFile { get; set; }

        public void SetYearSource(IEnumerable<DropDownItemViewModel> source)
            => YearSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => OrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetInputOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => InputOrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetCCPMDepTypeSource(IEnumerable<DropDownItemViewModel> source)
            => CCPMDepTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrgId = null)
            => Filter.OrganizationSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        //AddEmptySelectListItem()

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }
}
