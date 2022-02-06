using Datiss.Budget.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Datiss.Budget.Extensions;
using System.Linq;

namespace Datiss.Budget.ViewModels
{

    public class ReportViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EntityStatus Status { get; set; }
        public string StatusDisplay => Status.ToDisplay();
        public bool HasParameters { get; set; }

    }

    public class AdminReportFilterViewModel : FilterViewModel
    {

    }

    public class AdminReportIndexViewModel : PagedViewModel<ReportViewModel> { 
    
        public AdminReportIndexViewModel() {
            Filter = new AdminReportFilterViewModel();
            Items = new List<ReportViewModel>();
        }

        public AdminReportFilterViewModel Filter { get; set; }
    }


    public class CreateReportViewModel : BaseViewModel
    {

        public CreateReportViewModel() {
            Parameters = new List<CreateReportParamTypeViewModel>();
        }

        public void AddParam(CreateReportParamTypeViewModel p)
            => Parameters.Add(p);

        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public IList<CreateReportParamTypeViewModel> Parameters { get; set; }

        public IList<string> ParamNames { get; set; }
        public IList<int> ParamTypes { get; set; }
        public IList<SelectListItem> ParamTypeSource 
            => EnumSelectListProvider.GetReportParamTypes().ToList();

    }

    

    public class UpdateReportViewModel : BaseViewModel
    {

    }

}
