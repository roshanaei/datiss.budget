using Datiss.Budget.Entities.DWH;
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
    interface IUserTypeAverageCapacityService
    {
        Task<UserTypeAverageCapacity> GetByIdAsync(int id);

        //Task<ValidationResult> AddAsync(CreateUserTypeAverageCapacityDTO model);

        //Task<ValidationResult> UpdateAsync(UpdateUserTypeAverageCapacityDTO model);

        Task HardDeleteAsync(int Id);

        Task HardDeleteAsync(int yearId, int organizationId);

        //Task<int> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<UserTypeAverageCapacityDTO>> GetListAsync(UserTypeAverageCapacityFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(UserTypeAverageCapacityFilterDTO filter);

        Task<IEnumerable<UserTypeAverageCapacityDTO>> GetExportItemsAsync(UserTypeAverageCapacityFilterDTO filter);

        Task ImportExcelAsync(IFormFile fileInfo);
    }
}
