using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostForcastTransferWViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int TransferTypeId { get; set; }

        public string Location { get; set; }

        public int DigTypeId { get; set; }

        public int TubeTypeId { get; set; }

        public int DiameterPipeTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public int Lenth { get; set; }

        [Required(ErrorMessage = "*")]
        public long PipeCost { get; set; }

        [Required(ErrorMessage = "*")]
        public long RunCost { get; set; }

        [Required(ErrorMessage = "*")]
        public long TotalCost { get; set; }

        public int CreditTypeId { get; set; }

        public int ExtensionTypeId { get; set; }

        public int SuggestedBudgetTopicTypeId { get; set; }

        public IEnumerable<SelectListItem> TransferTypeSource { get; set; }
        public IEnumerable<SelectListItem> DigTypeSource { get; set; }
        public IEnumerable<SelectListItem> TubeTypeSource { get; set; }
        public IEnumerable<SelectListItem> DiameterPipeTypeSource { get; set; }
        public IEnumerable<SelectListItem> CreditTypeSource { get; set; }
        public IEnumerable<SelectListItem> ExtensionTypeSource { get; set; }
        public IEnumerable<SelectListItem> SuggestedBudgetTopicTypeSource { get; set; }

    }

    public class UpdateCostForcastTransferWViewModel : CreateCostForcastTransferWViewModel
    {
        public int Id { get; set; }

    }

    public class CostForcastTransferWViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int TransferTypeId { get; set; }
        public string TransferTypeDisplay { get; set; }

        public string Location { get; set; }


        public int DigTypeId { get; set; }
        public string DigTypeDisplay { get; set; }

        public int TubeTypeId { get; set; }
        public string TubeTypeDisplay { get; set; }

        public int Lenth { get; set; }
        public string LenthDisplay => Lenth.ToString("N0");

        public long PipeCost { get; set; }
        public string PipeCostDisplay => PipeCost.ToString("N0");

        public long RunCost { get; set; }
        public string RunCostDisplay => RunCost.ToString("N0");

        public long TotalCost { get; set; }
        public string TotalCostDisplay => TotalCost.ToString("N0");

        public int DiameterPipeTypeId { get; set; }
        public string DiameterPipeTypeDisplay { get; set; }

        public int CreditTypeId { get; set; }
        public string CreditDisplay { get; set; }

        public int ExtensionTypeId { get; set; }
        public string ExtensionDisplay { get; set; }

        public int SuggestedBudgetTopicTypeId { get; set; }
        public string SuggestedBudgetTopicDisplay { get; set; }
    }

    public class CostForcastTransferWFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostForcastTransferWIndexViewModel : PagedViewModel<CostForcastTransferWViewModel>
    {

        public CostForcastTransferWIndexViewModel()
        {
            Filter = new CostForcastTransferWFilterViewModel();
        }

        public CostForcastTransferWFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> TransferTypeSource { get; set; }
        public IList<SelectListItem> DigTypeSource { get; set; }
        public IList<SelectListItem> TubeTypeSource { get; set; }
        public IList<SelectListItem> DiameterPipeTypeSource { get; set; }
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

        public void SetTransferTypeSource(IEnumerable<DropDownItemViewModel> source)
            => TransferTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetDigTypeSource(IEnumerable<DropDownItemViewModel> source)
            => DigTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
        public void SetTubeTypeSource(IEnumerable<DropDownItemViewModel> source)
            => TubeTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetDiameterPipeTypeSource(IEnumerable<DropDownItemViewModel> source)
            => DiameterPipeTypeSource = source.Select(x => new SelectListItem
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

    public class CostForcastTransferWImportViewModel : PagedViewModel<CostForcastTransferWViewModel>
    {
        public IList<DropDownItemViewModel> TransferTypeSource { get; set; }
        public IList<DropDownItemViewModel> DigTypeSource { get; set; }
        public IList<DropDownItemViewModel> TubeTypeSource { get; set; }
        public IList<DropDownItemViewModel> DiameterPipeTypeSource { get; set; }
        public IList<DropDownItemViewModel> CreditTypeSource { get; set; }
        public IList<DropDownItemViewModel> ExtensionTypeSource { get; set; }
        public IList<DropDownItemViewModel> SuggestedBudgetTopicTypeSource { get; set; }

    }


}
