using System.Threading.Tasks;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Services.Infrastructure;

namespace Datiss.Budget.Services.Contracts.Identity
{
    public interface IUserService
    {

        Task<UserResultDTO> GetByIdAsync(int id);

        Task<ValidationResult<UserResultDTO>> CreateAsync(CreateUserDTO model);

        Task<ValidationResult<UserResultDTO>> UpdateAsync(UpdateUserDTO model);

        Task<PagedResult<UserResultDTO>> GetListAsync(UserFilterDTO filter);

        Task<bool> HasAccessToOrganizationAsync(int organizationId);

        Task SetUserStatusAsync(int id, EntityStatus status);

        Task SetUserPasswordAsync(int userId, string newPassword);
    }
}
