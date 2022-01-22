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
    public interface ISalesSplitTotalService
    {
        Task<SalesSplitTotal> GetByIdAsync(int id);

        Task<ValidationResult<SalesSplitTotalDTO>> CreateAsync(CreateSalesSplitTotalDTO model);

        Task<ValidationResult<SalesSplitTotalDTO>> UpdateAsync(UpdateSalesSplitTotalDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<SalesSplitTotalDTO>> GetListAsync(SalesSplitTotalFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(SalesSplitTotalFilterDTO filter);

        Task<IEnumerable<SalesSplitTotalDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
