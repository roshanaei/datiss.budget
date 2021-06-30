using Datiss.Budget.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Infrastructure;

namespace Datiss.Budget.Services
{
    public class WaterInstallFeeService : IWaterInstallFeeService
    {
        private readonly IUnitOfWork _uow;
        
        private DbSet<WaterInstallFee> _dbSet;

        public WaterInstallFeeService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<WaterInstallFee>();
        }

        public async Task<ValidationResult> AddAsync(AddWaterInstallFeeViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));
            var entity = new WaterInstallFee
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                DWaterTypeId = model.DWaterTypeId,
                WInstllFee = model.WInstllFee

            };

            await _dbSet.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return ValidationResult.Success();

        }

        public async Task<ValidationResult> UpdateAsync(UpdateWaterInstallFeeViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = await _dbSet.FindAsync(model.Id);
            entity.OrganizationId = model.OrganizationId;
            entity.YearId = model.YearId;
            entity.DWaterTypeId = model.DWaterTypeId;
            entity.WInstllFee = model.WInstllFee;

            await _uow.SaveChangesAsync();

            return ValidationResult.Success();
        }

        public async Task<ValidationResult> HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            entity.CheckArgumentIsNull(nameof(entity));

            //_dbSet.Remove(entity);

            return ValidationResult.Success();

        }




    }
}
