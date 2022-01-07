using System.Threading.Tasks;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Services.Contracts
{
    public interface IDataEntryTimeLimitService
    {

        Task CreateAsync(CreateDataEntryTimeLimitDTO model);

        Task CheckTimeLimitAsync(int? organizationId, int? yearId);
    }
}
