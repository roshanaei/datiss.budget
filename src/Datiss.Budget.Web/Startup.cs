using Datiss.Budget.ViewModels.Identity.Settings;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Datiss.Budget.IocConfig;
using DNTCommon.Web.Core;
using Microsoft.Extensions.Hosting;

namespace Datiss.Budget
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(options => Configuration.Bind(options));
            services.Configure<ContentSecurityPolicyConfig>(options => Configuration.GetSection("ContentSecurityPolicyConfig").Bind(options));

            services.AddMemoryCache();
            // Adds all of the ASP.NET Core Identity related services and configurations at once.
            services.AddCustomIdentityServices();
            services.AddDatissBudgetServices();

            services
                .AddMvc(options => options.UseYeKeModelBinder())
                .AddJsonOptions(jsonOptions =>
                    {
                        //jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                    });

            services.AddDNTCommonWeb();
            services.AddDNTCaptcha();
            services.AddCloudscribePagination();

            var mvc = services.AddControllersWithViews();

            if (Env.IsDevelopment()) {
                mvc.AddRazorRuntimeCompilation();
            }

            services.AddCors(options => {
                options.AddPolicy("EnableCORS", bl => {
                    bl.WithOrigins("localhost")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .Build();
                });
            });

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            //}
            //else {
            //    app.UseExceptionHandler("/error/500");
            //    app.UseStatusCodePagesWithReExecute("/error/{0}");
            //}

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseContentSecurityPolicy();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(policyName: "EnableCORS");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "areaRoute",
                pattern: "{area:exists}/{controller=Identity}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}