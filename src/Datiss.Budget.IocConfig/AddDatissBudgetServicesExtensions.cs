using Datiss.Budget.Services;
using Datiss.Budget.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AddDatissBudgetServicesExtensions
    {

        public static IServiceCollection AddDatissBudgetServices(this IServiceCollection services) {
            services.AddScoped<IConstantService, ConstantService>();

            return services;
        }
    }
}
