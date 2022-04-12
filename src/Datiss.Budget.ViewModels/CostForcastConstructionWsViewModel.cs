using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostForcastConstructionWsViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public string ProjectDescription { get; set; }

        public int WasteInvestorsTypeId { get; set; }

        public int CostCenterTypeId { get; set; }

        public int ExploitationAreaTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public int ProgressPercent { get; set; }

        [Required(ErrorMessage = "*")]
        public long CostDone { get; set; }

        [Required(ErrorMessage = "*")]
        public int Amount { get; set; }

        public int MeasurementTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public long UnitPrice { get; set; }

        [Required(ErrorMessage = "*")]
        public long TotalCost { get; set; }

        public int CreditTypeId { get; set; }

        public int ExtensionTypeId { get; set; }

        public int SuggestedBudgetTopicTypeId { get; set; }

        public IEnumerable<SelectListItem> WasteInvestorsTypeSource { get; set; }
        public IEnumerable<SelectListItem> CostCenterTypeSource { get; set; }
        public IEnumerable<SelectListItem> ExploitationAreaTypeSource { get; set; }
        public IEnumerable<SelectListItem> MeasurementTypeSource { get; set; }
        public IEnumerable<SelectListItem> CreditTypeSource { get; set; }
        public IEnumerable<SelectListItem> ExtensionTypeSource { get; set; }
        public IEnumerable<SelectListItem> SuggestedBudgetTopicTypeSource { get; set; }

    }

    public class UpdateCostForcastConstructionWsViewModel : CreateCostForcastConstructionWsViewModel
    {
        public int Id { get; set; }

    }

    public class CostForcastConstructionWsViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public string ProjectDescription { get; set; }

        public int WasteInvestorsTypeId { get; set; }
        public string WasteInvestorsDisplay { get; set; }

        public int CostCenterTypeId { get; set; }
        public string CostCenterDisplay { get; set; }

        public int ExploitationAreaTypeId { get; set; }
        public string ExploitationAreaDisplay { get; set; }

        public int ProgressPercent { get; set; }
        public string ProgressPercentDisplay => ProgressPercent.ToString("N0");

        public long CostDone { get; set; }
        public string CostDoneDisplay => CostDone.ToString("N0");

        public int Amount { get; set; }
        public string AmountDisplay => Amount.ToString("N0");

        public int MeasurementTypeId { get; set; }
        public string MeasurementDisplay { get; set; }

        public long UnitPrice { get; set; }
        public string UnitPriceDisplay => UnitPrice.ToString("N0");

        public long TotalCost { get; set; }
        public string TotalCostDisplay => TotalCost.ToString("N0");

        public int CreditTypeId { get; set; }
        public string CreditDisplay { get; set; }

        public int ExtensionTypeId { get; set; }
        public string ExtensionDisplay { get; set; }

        public int SuggestedBudgetTopicTypeId { get; set; }
        public string SuggestedBudgetTopicDisplay { get; set; }
    }

    public class CostForcastConstructionWsFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostForcastConstructionWsIndexViewModel : PagedViewModel<CostForcastConstructionWsViewModel>
    {

        public CostForcastConstructionWsIndexViewModel()
        {
            Filter = new CostForcastConstructionWsFilterViewModel();
        }

        public CostForcastConstructionWsFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> WasteInvestorsTypeSource { get; set; }
        public IList<SelectListItem> CostCenterTypeSource { get; set; }
        public IList<SelectListItem> ExploitationAreaTypeSource { get; set; }
        public IList<SelectListItem> MeasurementTypeSource { get; set; }
        public IList<SelectListItem> CreditTypeSource { get; set; }
        public IList<SelectListItem> ExtensionTypeSource { get; set; }
        public IList<SelectListItem> SuggestedBudgetTopicTypeSource { get; set; }

        public string ExtensionTypeSourceIdArray
        {
            get
            {
                if (ExtensionTypeSource == null || !ExtensionTypeSource.Any())
                    return string.Empty;
                string result = "";
                foreach (var item in ExtensionTypeSource)
                {
                    result += $"{item.Value},";
                }
                return result.TrimEnd(',');
            }
        }

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

        public void SetWasteInvestorsTypeSource(IEnumerable<DropDownItemViewModel> source)
            => WasteInvestorsTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetCostCenterTypeSource(IEnumerable<DropDownItemViewModel> source)
            => CostCenterTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        public void SetExploitationAreaTypeSource(IEnumerable<DropDownItemViewModel> source)
            => ExploitationAreaTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetMeasurementTypeSource(IEnumerable<DropDownItemViewModel> source)
            => MeasurementTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetCreditTypeSource(IEnumerable<DropDownItemViewModel> source)
            => CreditTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetExtensionTypeSource(IEnumerable<DropDownItemViewModel> source)
            => ExtensionTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetSuggestedBudgetTopicTypeSource(IEnumerable<DropDownItemViewModel> source)
            => SuggestedBudgetTopicTypeSource = source.Select(x => new SelectListItem
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

    public class CostForcastConstructionWsImportViewModel : PagedViewModel<CostForcastConstructionWsViewModel>
    {
        public IList<DropDownItemViewModel> WasteInvestorsTypeSource { get; set; }
        public IList<DropDownItemViewModel> CostCenterTypeSource { get; set; }
        public IList<DropDownItemViewModel> ExploitationAreaTypeSource { get; set; }
        public IList<DropDownItemViewModel> MeasurementTypeSource { get; set; }
        public IList<DropDownItemViewModel> CreditTypeSource { get; set; }
        public IList<DropDownItemViewModel> ExtensionTypeSource { get; set; }
        public IList<DropDownItemViewModel> SuggestedBudgetTopicTypeSource { get; set; }

    }


}
