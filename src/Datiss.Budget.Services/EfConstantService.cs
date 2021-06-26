using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Constant = Datiss.Budget.Entities.Constant;

namespace Datiss.Budget.Services
{
    public class EfConstantService : IEfConstantService
    {
        private readonly IUnitOfWork _uow;
        //private readonly IEfProductService _productService;
        private readonly DbSet<Constant> _constants;

        //public EfConstantService(IUnitOfWork uow, IEfProductService productService)
        //{
        //    _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        //    _productService = productService ?? throw new ArgumentNullException(nameof(productService));

        //    _constants = _uow.Set<Constant>();
        //}

        public void AddNewConstant(Constant constant)
        {
            _constants.Add(constant);
        }

        public void EditConstant(Constant model)
        {
            var constant = _constants.Single(x => x.Id == model.Id);
            if (constant != null)
            {
                constant.Title = model.Title;
                _uow.SaveChanges();
            }
        }

        public IList<Constant> GetAllConstants()
        {
            return _constants.ToList();
        }

        public async Task<IList<Constant>> GetAllConstantsAsync()
        {
            return await _constants.ToListAsync();
        }
    }
}
