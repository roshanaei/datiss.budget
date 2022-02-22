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

namespace Datiss.Budget.Services.Excel
{
    public interface ICostCurrentElectricityService
    {
        Task<CostCurrentElectricity> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentElectricityDTO>> CreateAsync(CreateCostCurrentElectricityDTO model);

        Task<ValidationResult<CostCurrentElectricityDTO>> UpdateAsync(UpdateCostCurrentElectricityDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId, ActivityType activityType);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<CostCurrentElectricityDTO>> GetListAsync(CostCurrentElectricityFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId, ActivityType activityType);

        Task<Stream> ExportExcelAsync(CostCurrentElectricityFilterDTO filter);

        Task<IEnumerable<CostCurrentElectricityDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, ActivityType activityType, bool continueIfAnyOrgMissing = false);

    }
}
