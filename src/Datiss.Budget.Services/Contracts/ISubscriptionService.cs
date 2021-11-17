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
    public interface ISubscriptionService
    {
        Task<Subscription> GetByIdAsync(int id);

        //Task<ValidationResult> AddAsync(CreateSubscriptionDTO model);

        //Task<ValidationResult> UpdateAsync(UpdateSubscriptionDTO model);

        Task HardDeleteAsync(int Id);

        Task HardDeleteAsync(int yearId, int organizationId);

        //Task<int> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<SubscriptionDTO>> GetListAsync(SubscriptionFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(SubscriptionFilterDTO filter);

        Task<IEnumerable<SubscriptionDTO>> GetExportItemsAsync(SubscriptionFilterDTO filter);

        Task ImportExcelAsync(IFormFile fileInfo);
    }
}
