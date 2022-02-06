using Datiss.Budget.Enum;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Datiss.Budget.ViewModels
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EntityStatus Status { get; set; }
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
    }

    

    public class UpdateReportViewModel : BaseViewModel
    {

    }

}
