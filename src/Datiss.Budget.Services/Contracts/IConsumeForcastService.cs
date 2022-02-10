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
   public interface  IConsumeForcastService
    {

        Task<ConsumeForcast> GetByIdAsync(int id);

        Task<ValidationResult<ConsumeForcastDTO>> CreateAsync(CreateConsumeForcastDTO model);

        Task<ValidationResult<ConsumeForcastDTO>> UpdateAsync(UpdateConsumeForcastDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<ConsumeForcastDTO>> GetListAsync(ConsumeForcastFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

        Task<IEnumerable<ConsumeForcastDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<Stream> ExportExcelAsync(ConsumeForcastFilterDTO filter);

    }
}
