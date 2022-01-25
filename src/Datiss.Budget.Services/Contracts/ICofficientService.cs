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
    public interface ICofficientService
    {
        Task<Cofficient> GetByIdAsync(int id);

        Task<ValidationResult<CofficientDTO>> CreateAsync(CreateCofficientDTO model);

        Task<ValidationResult<CofficientDTO>> UpdateAsync(UpdateCofficientDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<PagedResult<CofficientDTO>> GetListAsync(CofficientFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(CofficientFilterDTO filter);

        Task<IEnumerable<CofficientDTO>> GetExportItemsAsync(int yearId, int organizationId, CofficientsGroup groupname);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, CofficientsGroup group, bool continueIfAnyOrgMissing = false);
    }
}
