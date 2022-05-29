using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostForcastPipingWsViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int DigTypeId { get; set; }

        public int TubeTypeId { get; set; }

        public int DiameterPipeTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public long TubeBuyCost { get; set; }

        [Required(ErrorMessage = "*")]
        public long NaghabCost { get; set; }


        [Required(ErrorMessage = "*")]
        public long TeransheCost { get; set; }

        public IEnumerable<SelectListItem> DigTypeSource { get; set; }
        public IEnumerable<SelectListItem> TubeTypeSource { get; set; }
        public IEnumerable<SelectListItem> DiameterPipeTypeSource { get; set; }

    }

    public class UpdateCostForcastPipingWsViewModel : CreateCostForcastPipingWsViewModel
    {
        public int Id { get; set; }

    }

    public class CostForcastPipingWsViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }
        public int Year { get; set; }


        public int TubeTypeId { get; set; }
        public string TubeTypeDisplay { get; set; }

        public int DiameterPipeTypeId { get; set; }
        public string DiameterPipeTypeDisplay { get; set; }

        public int DigTypeId { get; set; }
        public string DigTypeDisplay { get; set; }


        public long TubeBuyCost { get; set; }
        public string TubeBuyCostDisplay => TubeBuyCost.ToString("N0");

        public long NaghabCost { get; set; }
        public string NaghabCostDisplay => NaghabCost.ToString("N0");

        public long TeransheCost { get; set; }
        public string TeransheCostDisplay => TeransheCost.ToString("N0");



    }

    public class CostForcastPipingWsFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
    }

    public class CostForcastPipingWsIndexViewModel : PagedViewModel<CostForcastPipingWsViewModel>
    {

        public CostForcastPipingWsIndexViewModel()
        {
            Filter = new CostForcastPipingWsFilterViewModel();
        }

        public CostForcastPipingWsFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> DigTypeSource { get; set; }
        public IList<SelectListItem> TubeTypeSource { get; set; }
        public IList<SelectListItem> DiameterPipeTypeSource { get; set; }


        public void SetYearSource(IEnumerable<DropDownItemViewModel> source)
            => YearSource = source.Select(x => new SelectListItem
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


        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }

    public class CostForcastPipingWsImportViewModel : PagedViewModel<CostForcastPipingWsViewModel>
    {
        public IList<DropDownItemViewModel> DigTypeSource { get; set; }
        public IList<DropDownItemViewModel> TubeTypeSource { get; set; }
        public IList<DropDownItemViewModel> DiameterPipeTypeSource { get; set; }

    }


}
