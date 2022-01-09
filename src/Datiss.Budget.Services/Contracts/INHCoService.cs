using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface INHCoService
    {
        Task<NHCo> GetByIdAsync(int id);

        Task<ValidationResult<NHCoDTO>> CreateAsync(CreateNHCoDTO model);

        Task<ValidationResult<NHCoDTO>> UpdateAsync(UpdateNHCoDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId, ActivityType activityType);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<NHCoDTO>> GetListAsync(NHCoFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId, ActivityType activityType);

        Task<Stream> ExportExcelAsync(NHCoFilterDTO filter);

        Task<IEnumerable<NHCoDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, ActivityType activityType, bool continueIfAnyOrgMissing = false);
    }
}
