using Datiss.Budget.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Datiss.Budget.Extensions;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Datiss.Budget.Resources;

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

    public class CreateReportViewModel : BaseViewModel {

        public CreateReportViewModel() {
            Params = new List<CreateReportParamTypeViewModel>();
        }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(100, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(255, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string Title { get; set; }

        [MaxLength(500, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string Description { get; set; }

        public IFormFile FileData { get; set; }

        public void ReadParams() {
            if (!ParamNames.Any() || !ParamTitles.Any() || !ParamTypes.Any())
                return;

            var names = ParamNames.Where(_ => _ != null).ToList();
            var titles = ParamTitles.Where(_ => _ != null).ToList();
            var paramTypes = ParamTypes.Skip(1).Take(ParamTypes.Count - 1).ToList();
            int i = 0;
            foreach (var n in names) {
                Params.Add(new CreateReportParamTypeViewModel
                {
                    Name = n,
                    ParamType = (ReportParamType)paramTypes[i],
                    Title = titles[i]
                });
                i++;
            }

        }

        public IList<CreateReportParamTypeViewModel> Params {
            get; set;
        }

        public IList<string> ParamNames { get; set; }
        public IList<string> ParamTitles { get; set; }
        public IList<int> ParamTypes { get; set; }
        public IList<SelectListItem> ParamTypeSource 
            => EnumSelectListProvider.GetReportParamTypes().ToList();

    }

    

    public class UpdateReportViewModel : CreateReportViewModel
    {
        public int Id { get; set; }

        public void LoadParams() {
            foreach(var p in Params) {
                ParamNames.Add(p.Name);
                ParamTypes.Add((int)p.ParamType);
                ParamTitles.Add(p.Title);
            }
        }
    }

}
