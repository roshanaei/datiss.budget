using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Entities;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Models;
using Mapster;

namespace Datiss.Budget.Services.Infrastructure
{
    public static class MapperConfig
    {

        public static void Config() 
        {
            TypeAdapterConfig<User, UserResultDTO>
                .NewConfig()
                .Map(d => d.OrganizationTitle, s => s.Organization != null ? s.Organization.Title : null)
                .Map(d => d.PositionTitle, s => s.Position != null ? s.Position.Title : null);

        }
    }
}
