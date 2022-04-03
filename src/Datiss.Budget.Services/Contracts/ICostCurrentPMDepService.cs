using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentPMDepService
    {
        Task<CostCurrentPMDep> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentPMDepDTO>> UpdateAsync(UpdateCostCurrentPMDepDTO model);

        Task<PagedResult<CostCurrentPMDepDTO>> GetListAsync(CostCurrentPMDepFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<ValidationResult> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<IEnumerable<CostCurrentPMDepDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
