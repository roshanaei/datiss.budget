using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Entities.DWH;
using Microsoft.AspNetCore.Http;
using System.IO;
using Datiss.Budget.Entities;

namespace Datiss.Budget.Services.Contracts
{
    public interface IConsumeForcastWsService
    {
        Task<ConsumeForcastWs> GetByIdAsync(int id);

        Task<ValidationResult<ConsumeForcastWsDTO>> CreateAsync(CreateConsumeForcastWsDTO model);

        Task<ValidationResult<ConsumeForcastWsDTO>> UpdateAsync(UpdateConsumeForcastWsDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<ConsumeForcastWsDTO>> GetListAsync(ConsumeForcastWsFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

        Task<IEnumerable<ConsumeForcastWsDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<Stream> ExportExcelAsync(ConsumeForcastWsFilterDTO filter);
    }
}
