using Datiss.Budget.Entities;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface IEfWaterInstallFeeService
    {
        void AddNewWaterInstallFee(WaterInstallFee waterInstallFee);
        IList<WaterInstallFee> GetAllWaterInstallFees();

        void EditWaterInstallFee(WaterInstallFee model);
        Task<IList<WaterInstallFee>> GetAllWaterInstallFeesAsync();



        //Task<ServiceActionResult<WaterInstallFee>> AddApiAsync(WaterInstallFee model);
        //Task<List<WaterInstallFeeApiModel>> GetAllApiAsync();
    }

}
