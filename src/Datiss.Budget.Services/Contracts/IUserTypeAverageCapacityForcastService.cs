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
    public interface IUserTypeAverageCapacityForcastService
    {
        Task<UserTypeAverageCapacityForcast> GetByIdAsync(int id);

        Task<ValidationResult<UserTypeAverageCapacityForcastDTO>> CreateAsync(CreateUserTypeAverageCapacityForcastDTO model);

        Task<ValidationResult<UserTypeAverageCapacityForcastDTO>> UpdateAsync(UpdateUserTypeAverageCapacityForcastDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<PagedResult<UserTypeAverageCapacityForcastDTO>> GetListAsync(UserTypeAverageCapacityForcastFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(UserTypeAverageCapacityForcastFilterDTO filter);

        Task<IEnumerable<UserTypeAverageCapacityForcastDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
