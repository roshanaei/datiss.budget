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

namespace Datiss.Budget.Services.Contracts
{
    public interface IUserTypeAverageCapacityService
    {
        Task<UserTypeAverageCapacity> GetByIdAsync(int id);

        Task<ValidationResult<UserTypeAverageCapacityDTO>> CreateAsync(CreateUserTypeAverageCapacityDTO model);

        Task<ValidationResult<UserTypeAverageCapacityDTO>> UpdateAsync(UpdateUserTypeAverageCapacityDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<PagedResult<UserTypeAverageCapacityDTO>> GetListAsync(UserTypeAverageCapacityFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(UserTypeAverageCapacityFilterDTO filter);

        Task<IEnumerable<UserTypeAverageCapacityDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
