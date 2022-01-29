using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
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
    public class CreatePerformanceEvaluationViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public EntityStatus Status { get; set; }

        public int TableFieldId { get; set; }

        public decimal Target { get; set; }

        public decimal Operation { get; set; }

    }

    public class UpdatePerformanceEvaluationViewModel : CreatePerformanceEvaluationViewModel
    {
        public int Id { get; set; }

    }

    public class PerformanceEvaluationViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public bool Status { get; set; }
        public int TableFieldId { get; set; }
        public string TableFieldDisplay { get; set; }
        public decimal Target { get; set; }
        public string TargetDisplay => Target.ToString("N2");
        public decimal Operation { get; set; }
        public string OperationDisplay => Operation.ToString("N2");
        public decimal Budget { get; set; }
        public string BudgetDisplay => Budget.ToString("N2");
        public decimal PercentRealization { get; set; }
        public string PercentRealizationDisplay => PercentRealization.ToString("N2");

    }

    public class PerformanceEvaluationFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public TablesName? TableName { get; set; }
        public SectionName? SectionName { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> SectionNameSource => EnumSelectListProvider.GetSectionNameTypeItem(SectionName).ToList().AddEmptySelectListItem();
    }

    public class PerformanceEvaluationIndexViewModel : PagedViewModel<PerformanceEvaluationViewModel>
    {

        public PerformanceEvaluationIndexViewModel()
        {
            Filter = new PerformanceEvaluationFilterViewModel();
        }

        public PerformanceEvaluationFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }


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

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrgId = null)
            => Filter.OrganizationSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();


        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }
}
