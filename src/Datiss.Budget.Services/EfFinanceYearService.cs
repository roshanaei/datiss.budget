using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FinanceYear = Datiss.Budget.Entities.FinanceYear;

namespace Datiss.Budget.Services
{
    public class EfFinanceYearService : IEfFinanceYearService
    {
        private readonly IUnitOfWork _uow;
        //private readonly IEfProductService _productService;
        private readonly DbSet<FinanceYear> _financeyears;

        //public EfFinanceYearService(IUnitOfWork uow, IEfProductService productService)
        //{
        //    _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        //    _productService = productService ?? throw new ArgumentNullException(nameof(productService));

        //    _financeyears = _uow.Set<FinanceYear>();
        //}

        public void AddNewFinanceYear(FinanceYear financeyear)
        {
            _financeyears.Add(financeyear);
        }

        public void EditFinanceYear(FinanceYear model)
        {
            var financeyear = _financeyears.Single(x => x.Id == model.Id);
            if (financeyear != null)
            {
                financeyear.Title = model.Title;
                _uow.SaveChanges();
            }
        }

        public IList<FinanceYear> GetAllFinanceYears()
        {
            return _financeyears.ToList();
        }

        public async Task<IList<FinanceYear>> GetAllFinanceYearsAsync()
        {
            return await _financeyears.ToListAsync();
        }
    }
}
