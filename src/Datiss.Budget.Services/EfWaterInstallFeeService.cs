using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WaterInstallFee = Datiss.Budget.Entities.WaterInstallFee;

namespace Datiss.Budget.Services
{
    public class EfWaterInstallFeeService : IEfWaterInstallFeeService
    {
        private readonly IUnitOfWork _uow;
        //private readonly IEfProductService _productService;
        private readonly DbSet<WaterInstallFee> _waterInstallFees;

        //public EfWaterInstallFeeService(IUnitOfWork uow, IEfProductService productService)
        //{
        //    _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        //    _productService = productService ?? throw new ArgumentNullException(nameof(productService));

        //    _waterInstallFees = _uow.Set<WaterInstallFee>();
        //}

        public void AddNewWaterInstallFee(WaterInstallFee waterInstallFee)
        {
            _waterInstallFees.Add(waterInstallFee);
        }

        public void EditWaterInstallFee(WaterInstallFee model)
        {
            var waterInstallFee = _waterInstallFees.Single(x => x.Id == model.Id);
            if (waterInstallFee != null)
            {
                waterInstallFee.WInstllFee = model.WInstllFee;
                _uow.SaveChanges();
            }
        }

        public IList<WaterInstallFee> GetAllWaterInstallFees()
        {
            return _waterInstallFees.ToList();
        }

        public async Task<IList<WaterInstallFee>> GetAllWaterInstallFeesAsync()
        {
            return await _waterInstallFees.ToListAsync();
        }
    }
}
