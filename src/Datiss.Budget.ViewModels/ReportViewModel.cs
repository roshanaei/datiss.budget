using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Enum;

namespace Datiss.Budget.ViewModels
{
    public class ReportViewModel
    {
        public ReportViewModel()
            => Params = new List<ReportParamViewModel>();

        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EntityStatus Status { get; set; }
        public byte[] FileData { get; set; }
        public IList<ReportParamViewModel> Params { get; set; }
    }

    public class ReportDisplayViewModel : BaseViewModel
    {
        public ReportDisplayViewModel() {
            Report = new ReportViewModel();
            YearSource = new List<SelectListItem>();
            OrganizationSource = new List<SelectListItem>();
            CitySource = new List<SelectListItem>();
            CountySource = new List<SelectListItem>();
            VillageSource = new List<SelectListItem>();
        }

        public bool DisplayReport { get; set; }

        public ReportViewModel Report { get; set; }

        public IEnumerable<SelectListItem> YearSource { get; private set; }

        public IEnumerable<SelectListItem> OrganizationSource { get; private set; }

        public IEnumerable<SelectListItem> CitySource { get; private set; }

        public IEnumerable<SelectListItem> CountySource { get; private set; }

        public IEnumerable<SelectListItem> VillageSource { get; private set; }

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

        public void SetCitySource(IEnumerable<DropDownItemViewModel> source)
            => CitySource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetCountySource(IEnumerable<DropDownItemViewModel> source)
            => CountySource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetVillageSource(IEnumerable<DropDownItemViewModel> source)
           => VillageSource = source.Select(x => new SelectListItem
           {
               Text = x.Title,
               Value = x.Id.ToString()
           }).ToList();

    }

    public class ReportFilterViewModel : FilterViewModel
    {

    }

    public class ReportIndexViewModel : PagedViewModel<AdminReportViewModel>
    {

        public ReportIndexViewModel() {
            Filter = new ReportFilterViewModel();
            Items = new List<AdminReportViewModel>();
        }

        public ReportFilterViewModel Filter { get; set; }
    }

}
