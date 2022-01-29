using System.Linq;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Models;
using Mapster;

namespace Datiss.Budget.Services.Infrastructure
{
    public static class MapperConfig
    {
        /// <summary>
        /// Config default maps for mapster. You should call this method in DI before using project's services.
        /// </summary>
        public static void Config() 
        {
            TypeAdapterConfig<User, UserResultDTO>
                .NewConfig()
                .Map(d => d.OrganizationTitle, s => s.Organization != null ? s.Organization.Title : null)
                .Map(d => d.PositionTitle, s => s.Position != null ? s.Position.Title : null)
                .Map(d => d.SelectedRoles, s => s.Roles != null && s.Roles.Any() 
                                                ? s.Roles.Select(_ => _.RoleId).ToList() 
                                                : null);

        }
    }
}
