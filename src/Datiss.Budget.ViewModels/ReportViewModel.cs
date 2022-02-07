using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
