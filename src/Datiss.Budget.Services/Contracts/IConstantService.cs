using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Services.Contracts
{
    public interface IConstantService
    {
        Task<Constant> GetByIdAsync(int id);

        Task<ValidationResult> CreateAsync(CreateConstantDTO model);

        Task<ValidationResult> UpdateAsync(UpdateConstantDTO model);

        Task<ValidationResult> SoftDeleteAsync(int id);

        Task<IEnumerable<DropDownItem>> GetParentsAsync();

        Task<IEnumerable<DropDownItem>> GetByConstantKeyAsync(string key, EntityStatus? status = EntityStatus.Deleted);

        Task<IEnumerable<DropDownItem>> GetByKeyAsync(string key, string parentkey, bool none = false);
        Task<IEnumerable<DropDownItem>> GetCofficientByKeysAsync(string key, string parentkey);

        Task<IEnumerable<ConstantDTO>> GetDataByKeyAsync(string key);

        Task<IEnumerable<DropDownItem>> GetRecordsByKeyAsynce(string parentkey, string key);

        Task<PagedResult<ConstantDTO>> GetListAsync(ConstantFilterDTO filter);
    }
}
