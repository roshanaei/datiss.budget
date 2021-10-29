using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;


namespace Datiss.Budget.ViewModels
{
    public class FinanceYearIndexViewModel
    {
        public FinanceYearIndexViewModel()
        {
            Model = new PagedResult<FinanceYearViewModel>();
        }

        public PagedResult<FinanceYearViewModel> Model { get; set; }

    }
}
