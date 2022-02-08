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

namespace Datiss.Budget.Services
{
    public interface IIncomeCurrentReportService
    {
        Task<IncomeCurrentReport> GetByIdAsync(int id);
        Task<ValidationResult<IncomeCurrentReportDTO>> UpdateAsync(UpdateIncomeCurrentReportDTO model);
        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);
        Task<PagedResult<IncomeCurrentReportDTO>> GetListAsync(IncomeCurrentReportFilterDTO filter);
        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);
        Task<Stream> ExportExcelAsync(IncomeCurrentReportFilterDTO filter);
        Task<IEnumerable<IncomeCurrentReportDTO>> GetExportItemsAsync(int yearId, int organizationId);
        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
