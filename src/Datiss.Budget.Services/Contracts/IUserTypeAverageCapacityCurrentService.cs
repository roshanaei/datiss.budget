using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface IUserTypeAverageCapacityCurrentService
    {
        Task<UserTypeAverageCapacityCurrent> GetByIdAsync(int id);

        Task<ValidationResult<UserTypeAverageCapacityCurrentDTO>> CreateAsync(CreateUserTypeAverageCapacityCurrentDTO model);

        Task<ValidationResult<UserTypeAverageCapacityCurrentDTO>> UpdateAsync(UpdateUserTypeAverageCapacityCurrentDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<PagedResult<UserTypeAverageCapacityCurrentDTO>> GetListAsync(UserTypeAverageCapacityCurrentFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(UserTypeAverageCapacityCurrentFilterDTO filter);

        Task<IEnumerable<UserTypeAverageCapacityCurrentDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
