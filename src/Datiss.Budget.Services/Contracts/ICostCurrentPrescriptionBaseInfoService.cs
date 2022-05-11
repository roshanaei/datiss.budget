using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Entities.DWH;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentPrescriptionBaseInfoService
    {
        Task<CostCurrentPrescriptionBaseInfo> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentPrescriptionBaseInfoDTO>> CreateAsync(CreateCostCurrentPrescriptionBaseInfoDTO model);

        Task<ValidationResult<CostCurrentPrescriptionBaseInfoDTO>> UpdateAsync(UpdateCostCurrentPrescriptionBaseInfoDTO model);

        Task HardDeleteAsync(int Id);

        Task<SubscriptionDeleteDataResult> HardDeleteAllAsync(int yearId);

        Task<PagedResult<CostCurrentPrescriptionBaseInfoDTO>> GetListAsync(CostCurrentPrescriptionBaseInfoFilterDTO filter);

        Task CopyAsync(int sourceYearId, int destYearId);

        Task<Stream> ExportExcelAsync(CostCurrentPrescriptionBaseInfoFilterDTO filter);

        Task<IEnumerable<CostCurrentPrescriptionBaseInfoDTO>> GetExportItemsAsync(int yearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId);
    }
}
