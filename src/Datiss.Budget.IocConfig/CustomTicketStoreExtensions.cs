using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Datiss.Budget.IocConfig
{
    public static class CustomTicketStoreExtensions
    {
        public static IServiceCollection AddCustomTicketStore(
            this IServiceCollection services, SiteSettings siteSettings)
        {
            // To manage large identity cookies
            var cookieOptions = siteSettings.CookieOptions;
            if (!cookieOptions.UseDistributedCacheTicketStore)
            {
                return services;
            }

            services.AddDistributedSqlServerCache(options =>
            {
                var cacheOptions = cookieOptions.DistributedSqlServerCacheOptions;
                options.ConnectionString = string.IsNullOrWhiteSpace(cacheOptions.ConnectionString) ?
                        siteSettings.GetApplicationDbContextDbConnectionString() :
                        cacheOptions.ConnectionString;
                options.SchemaName = cacheOptions.SchemaName;
                options.TableName = cacheOptions.TableName;
            });
            services.AddScoped<ITicketStore, DistributedCacheTicketStore>();

            return services;
        }
    }
}