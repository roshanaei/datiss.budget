using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostForcastBuyDescriptionViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int AssetTypeId { get; set; }

        public int AssetDetailTypeId { get; set; }

        public int MeasurementTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public long UnitPrice { get; set; }


        public IEnumerable<SelectListItem> AssetTypeSource { get; set; }
        public IEnumerable<SelectListItem> AssetDetailSource { get; set; }
        public IEnumerable<SelectListItem> MeasurementTypeSource { get; set; }

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

    public class CostForcastBuyDescriptionIndexViewModel : PagedViewModel<CostForcastPipingWViewModel>
    {

        public CostForcastBuyDescriptionIndexViewModel()
        {
            Filter = new CostForcastBuyDescriptionFilterViewModel();
        }

        public CostForcastBuyDescriptionFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> AssetTypeSource { get; set; }
        public IList<SelectListItem> AssetDetailSource { get; set; }
        public IList<SelectListItem> MeasurementTypeSource { get; set; }


        public void SetYearSource(IEnumerable<DropDownItemViewModel> source)
            => YearSource = source.Select(x => new SelectListItem
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
        public void SetAssetDetailSource(IEnumerable<DropDownItemViewModel> source)
            => AssetDetailSource = source.Select(x => new SelectListItem
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


        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }

    public class CostForcastBuyDescriptionImportViewModel : PagedViewModel<CostForcastBuyDescriptionViewModel>
    {
        public IList<DropDownItemViewModel> AssetTypeSource { get; set; }
        public IList<DropDownItemViewModel> AssetDetailSource { get; set; }
        public IList<DropDownItemViewModel> MeasurementTypeSource { get; set; }

    }


}
