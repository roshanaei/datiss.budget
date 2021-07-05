using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Models;


namespace Datiss.Budget.ViewModels
{
    public class WaterInstallFeeIndexViewModel
    {
        public WaterInstallFeeIndexViewModel() {
            Model = new PagedResult<WaterInstallFeeViewModel>();
            Filter = new WaterInstallFeeFilterViewModel();
        }

        public PagedResult<WaterInstallFeeViewModel> Model { get; set; }

        public WaterInstallFeeFilterViewModel Filter { get; set; }
    }
}
