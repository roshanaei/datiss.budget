using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostForcastBuyViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public string BuyDescription { get; set; }

        [Required(ErrorMessage = "*")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "*")]
        public int BuyDepartmentId { get; set; }

        [Required(ErrorMessage = "*")]
        public int CostCenterTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public int AssetTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public int AssetDetailTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "*")]
        public int MeasurementTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public long UnitPrice { get; set; }

        [Required(ErrorMessage = "*")]
        public int CreditTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public long ProposedCost { get; set; }

        //public IEnumerable<SelectListItem> MeasurementTypeSource { get; set; }
        //public IEnumerable<SelectListItem> BuyDepartmentTypeSource { get; set; }
        //public IEnumerable<SelectListItem> CostCenterTypeSource { get; set; }
        //public IEnumerable<SelectListItem> CreditTypeSource { get; set; }
        //public IEnumerable<SelectListItem> AssetTypeSource { get; set; }
        //public IEnumerable<SelectListItem> AssetDetailTypeSource { get; set; }

    }

    public class UpdateCostForcastBuyViewModel : CreateCostForcastBuyViewModel
    {
        public int Id { get; set; }

    }

    public class CostForcastBuyViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public string BuyDescription { get; set; }

        public int LocationId { get; set; }
        public string LocationDisplay { get; set; }

        public int BuyDepartmentId { get; set; }
        public string BuyDepartmentDisplay { get; set; }

        public int CostCenterTypeId { get; set; }
        public string CostCenterTypeDisplay { get; set; }

        public int AssetTypeId { get; set; }
        public string AssetTypeDisplay { get; set; }

        public int AssetDetailTypeId { get; set; }
        public string AssetDetailTypeDisplay { get; set; }

        public int Amount { get; set; }
        public string AmountDisplay => Amount.ToString("N0");

        public int MeasurementTypeId { get; set; }
        public string MeasurementTypeDisplay { get; set; }

        public long UnitPrice { get; set; }
        public string UnitPriceDisplay => UnitPrice.ToString("N0");

        public int CreditTypeId { get; set; }
        public string CreditTypeDisplay { get; set; }

        public long ProposedCost { get; set; }
        public string ProposedCostDisplay => ProposedCost.ToString("N0");
    }

    public class CostForcastBuyFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? LocationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class CostForcastBuyIndexViewModel : PagedViewModel<CostForcastBuyViewModel>
    {

        public CostForcastBuyIndexViewModel()
        {
            Filter = new CostForcastBuyFilterViewModel();
        }

        public CostForcastBuyFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> MeasurementTypeSource { get; set; }
        public IList<SelectListItem> LocationTypeSource { get; set; }
        public IList<SelectListItem> BuyDepartmentTypeSource { get; set; }
        public IList<SelectListItem> CostCenterTypeSource { get; set; }
        public IList<SelectListItem> CreditTypeSource { get; set; }
        public IList<SelectListItem> AssetTypeSource { get; set; }
        public IList<SelectListItem> AssetDetailTypeSource { get; set; }

        public string AssetTypeSourceIdArray
        {
            get
            {
                if (AssetTypeSource == null || !AssetTypeSource.Any())
                    return string.Empty;
                string result = "";
                foreach (var item in AssetTypeSource)
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

        public void SetLocationTypeSource(IEnumerable<DropDownItemViewModel> source)
            => LocationTypeSource = source.Select(x => new SelectListItem
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

        public void SetBuyDepartmentTypeSource(IEnumerable<DropDownItemViewModel> source)
            => BuyDepartmentTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SeCostCenterTypeSource(IEnumerable<DropDownItemViewModel> source)
            => CostCenterTypeSource = source.Select(x => new SelectListItem
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

        public void SetAssetTypeSource(IEnumerable<DropDownItemViewModel> source)
            => AssetTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetAssetDetailTypeSource(IEnumerable<DropDownItemViewModel> source)
            => AssetDetailTypeSource = source.Select(x => new SelectListItem
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

    public class CostForcastBuyImportViewModel : PagedViewModel<CostForcastBuyViewModel>
    {
        public IList<DropDownItemViewModel> LocationTypeSource { get; set; }
        public IList<DropDownItemViewModel> MeasurementTypeSource { get; set; }
        public IList<DropDownItemViewModel> BuyDepartmentTypeSource { get; set; }
        public IList<DropDownItemViewModel> CostCenterTypeSource { get; set; }
        public IList<DropDownItemViewModel> CreditTypeSource { get; set; }
        public IList<DropDownItemViewModel> AssetTypeSource { get; set; }
        public IList<DropDownItemViewModel> AssetDetailTypeSource { get; set; }

    }


}