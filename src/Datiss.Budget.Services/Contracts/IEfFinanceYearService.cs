using Datiss.Budget.Entities;
using Datiss.Budget.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface IEfFinanceYearService
    {
        void AddNewFinanceYear(FinanceYear financeyear);
        IList<FinanceYear> GetAllFinanceYears();

        void EditFinanceYear(FinanceYear model);
        Task<IList<FinanceYear>> GetAllFinanceYearsAsync();



        //Task<ServiceActionResult<FinanceYear>> AddApiAsync(FinanceYear model);
        //Task<List<FinanceYearApiModel>> GetAllApiAsync();
    }

}
