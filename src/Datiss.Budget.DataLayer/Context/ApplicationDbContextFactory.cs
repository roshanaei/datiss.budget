using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Datiss.Budget.DataLayer.Context;
using System;
using System.IO;
using Datiss.Budget.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Datiss.Budget.DataLayer.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            var basePath = Directory.GetCurrentDirectory();
            Console.WriteLine($"Using `{basePath}` as the ContentRootPath");
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(basePath)
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .Build();
            services.AddSingleton<IConfigurationRoot>(provider => configuration);
            services.Configure<SiteSettings>(options => configuration.Bind(options));

            var siteSettings = services.BuildServiceProvider().GetRequiredService<IOptionsSnapshot<SiteSettings>>();
            //siteSettings.Value.ActiveDatabase = ActiveDatabase.LocalDb;

            services.AddEntityFrameworkSqlServer(); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseConfiguredApplicationDbContext(siteSettings.Value, services.BuildServiceProvider());

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}