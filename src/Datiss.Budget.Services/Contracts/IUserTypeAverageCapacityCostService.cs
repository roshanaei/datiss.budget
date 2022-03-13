using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface IUserTypeAverageCapacityCostService
    {
        Task<UserTypeAverageCapacityCost> GetByIdAsync(int id);

        Task<ValidationResult<UserTypeAverageCapacityCostDTO>> CreateAsync(CreateUserTypeAverageCapacityCostDTO model);

        Task<ValidationResult<UserTypeAverageCapacityCostDTO>> UpdateAsync(UpdateUserTypeAverageCapacityCostDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<PagedResult<UserTypeAverageCapacityCostDTO>> GetListAsync(UserTypeAverageCapacityCostFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(UserTypeAverageCapacityCostFilterDTO filter);

        Task<IEnumerable<UserTypeAverageCapacityCostDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
