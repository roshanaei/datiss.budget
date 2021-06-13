using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.ViewModels.Identity.Settings;
using DNTCommon.Web.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Datiss.Budget.IocConfig
{
    public static class DbContextOptionsExtensions
    {
        public static IServiceCollection AddConfiguredDbContext(
            this IServiceCollection serviceCollection, SiteSettings siteSettings)
        {
            serviceCollection.AddConfiguredApplicationDbContext(siteSettings);

            return serviceCollection;
        }

        /// <summary>
        /// Creates and seeds the database.
        /// </summary>
        public static void InitializeDb(this IServiceProvider serviceProvider)
        {
            serviceProvider.RunScopedService<IIdentityDbInitializer>(identityDbInitialize =>
            {
                identityDbInitialize.Initialize();
                identityDbInitialize.SeedData();
            });
        }
    }
}