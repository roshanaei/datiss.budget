using System;
using System.IO;
using Datiss.Budget.Common.PersianToolkit;
using Datiss.Budget.Common.WebToolkit;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.ViewModels.Identity.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Datiss.Budget.DataLayer.Context
{
    public static class ApplicationDbContextServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguredApplicationDbContext(this IServiceCollection services, SiteSettings siteSettings)
        {
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
            services.AddEntityFrameworkSqlServer(); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            //services.AddDbContextPool<ApplicationDbContext, MsSqlDbContext>(
            services.AddDbContextPool<ApplicationDbContext>(
                (serviceProvider, optionsBuilder) => optionsBuilder.UseConfiguredApplicationDbContext(siteSettings, serviceProvider));
            return services;
        }

        public static void UseConfiguredApplicationDbContext(
            this DbContextOptionsBuilder optionsBuilder, SiteSettings siteSettings, IServiceProvider serviceProvider)
        {
            var connectionString = siteSettings.GetApplicationDbContextDbConnectionString();
            optionsBuilder.UseSqlServer(
                        connectionString,
                        sqlServerOptionsBuilder =>
                        {
                            sqlServerOptionsBuilder.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
                            sqlServerOptionsBuilder.EnableRetryOnFailure();
                            sqlServerOptionsBuilder.MigrationsAssembly(typeof(ApplicationDbContextServiceCollectionExtensions).Assembly.FullName);
                        });
            optionsBuilder.UseInternalServiceProvider(serviceProvider); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            optionsBuilder.AddInterceptors(new PersianYeKeCommandInterceptor());
            optionsBuilder.ConfigureWarnings(warnings =>
            {
                // ...
            });
        }

        public static string GetApplicationDbContextDbConnectionString(this SiteSettings siteSettingsValue)
        {
            if (siteSettingsValue == null)
            {
                throw new ArgumentNullException(nameof(siteSettingsValue));
            }

            return siteSettingsValue.ConnectionStrings.ApplicationDbContextConnection;
        }
    }
}