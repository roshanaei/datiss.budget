using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Entities.Identity;
using System.Security.Claims;
using Datiss.Budget.ViewModels.Identity;

namespace Datiss.Budget.Services.Contracts.Identity
{
    public interface ISiteStatService
    {
        Task<List<User>> GetOnlineUsersListAsync(int numbersToTake, int minutesToTake);

        Task<List<User>> GetTodayBirthdayListAsync();

        Task UpdateUserLastVisitDateTimeAsync(ClaimsPrincipal claimsPrincipal);

        Task<AgeStatViewModel> GetUsersAverageAge();
    }
}