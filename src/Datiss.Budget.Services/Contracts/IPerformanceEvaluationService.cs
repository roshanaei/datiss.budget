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
    public interface IPerformanceEvaluationService
    {
        Task<PerformanceEvaluation> GetByIdAsync(int id);
        Task<ValidationResult<PerformanceEvaluationDTO>> UpdateAsync(UpdatePerformanceEvaluationDTO model);
        Task<OrganizationDeleteDataResult> SoftDeleteAsync(int yearId, int organizationId , TablesName tablesName);
        Task<PagedResult<PerformanceEvaluationDTO>> GetListAsync(PerformanceEvaluationFilterDTO filter);
        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, TablesName tablesName, bool continueIfAnyOrgMissing = false);
        Task<IEnumerable<PerformanceEvaluationDTO>> GetExportItemsAsync(int yearId, int organizationId, TablesName tablesName);

    }
}
