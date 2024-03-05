using Microsoft.AspNetCore.Hosting;
using Datiss.Budget.Services.Identity.Logger;
using Datiss.Budget.IocConfig;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using Serilog;
using Datiss.Budget.Web.Core;
using System;

namespace Datiss.Budget {

    public class Program {

        public static void Main(string[] args)
        {
            //Log.Logger = SerilogConfiguration.CreateLogger(seqUrl: "http://localhost:5341/");

            try {
                //Log.Information("Starting web host for Datiss.Budget");
                var host = CreateHostBuilder(args).Build();
                host.Services.InitializeDb();
                host.Run();
            }
            catch(Exception ex) {
                throw ex;
                //Log.Fatal(ex, "Host of Datiss.Budget terminated unexpectedly");
            }
            finally {
                //Log.CloseAndFlush();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging((hostingContext, logging) =>
                               {
                                   logging.ClearProviders();

                                   logging.AddDebug();

                                   if (hostingContext.HostingEnvironment.IsDevelopment())
                                   {
                                       logging.AddConsole();
                                   }

                                   logging.AddDbLogger(); // You can change its Log Level using the `appsettings.json` file -> Logging -> LogLevel -> Default
                                   logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                               })
                              .UseStartup<Startup>();
                              //.UseSerilog();
                });
    }
}