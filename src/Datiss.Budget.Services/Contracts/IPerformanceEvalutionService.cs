using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface IPerformanceEvalutionService
    {
        Task<PerformanceEvaluation> GetByIdAsync(int id);
        Task<ValidationResult<PerformanceEvaluationDTO>> CreateAsync(CreatePerformanceEvaluationDTO model);
        Task<ValidationResult<PerformanceEvaluationDTO>> UpdateAsync(UpdatePerformanceEvaluationDTO model);
        Task SoftDeleteAsync(int Id);
        Task<PagedResult<PerformanceEvaluationDTO>> GetListAsync(PerformanceEvaluationFilterDTO filter);
        Task ImportExcelAsync(IFormFile fileInfo);
    }
}
