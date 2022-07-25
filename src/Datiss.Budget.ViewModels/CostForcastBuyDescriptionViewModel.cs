using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostForcastBuyDescriptionViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int AssetTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public int AssetDetailTypeId { get; set; }

        [Required(ErrorMessage = "*")]

        public int MeasurementTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public long UnitPrice { get; set; }



        //public IEnumerable<SelectListItem> MeasurementTypeSource { get; set; }
        //public IEnumerable<SelectListItem> BuyDepartmentTypeSource { get; set; }
        //public IEnumerable<SelectListItem> CostCenterTypeSource { get; set; }
        //public IEnumerable<SelectListItem> CreditTypeSource { get; set; }
        //public IEnumerable<SelectListItem> AssetTypeSource { get; set; }
        //public IEnumerable<SelectListItem> AssetDetailTypeSource { get; set; }

    }

    public class UpdateCostForcastBuyDescriptionViewModel : CreateCostForcastBuyDescriptionViewModel
    {
        public int Id { get; set; }

    }

    public class CostForcastBuyDescriptionViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }

        public int AssetTypeId { get; set; }
        public string AssetTypeDisplay { get; set; }

        public int AssetDetailTypeId { get; set; }
        public string AssetDetailTypeDisplay { get; set; }

        public int MeasurementTypeId { get; set; }
        public string MeasurementTypeDisplay { get; set; }

        public long UnitPrice { get; set; }
        public string UnitPriceDisplay => UnitPrice.ToString("N0");

    }

    public class CostForcastBuyDescriptionFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

    }

    public class CostForcastBuyDescriptionIndexViewModel : PagedViewModel<CostForcastBuyDescriptionViewModel>
    {

        public CostForcastBuyDescriptionIndexViewModel()
        {
            Filter = new CostForcastBuyDescriptionFilterViewModel();
        }

        public CostForcastBuyDescriptionFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> MeasurementTypeSource { get; set; }
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

        public void SetMeasurementTypeSource(IEnumerable<DropDownItemViewModel> source)
            => MeasurementTypeSource = source.Select(x => new SelectListItem
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

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }

    public class CostForcastBuyDescriptionImportModel
    {
        public IList<DropDownItemViewModel> MeasurementTypeSource { get; set; }

        public List<CostForcastBuyDescriptionViewModel> CostForcastBuyDescriptions { get; set; }

    }


}