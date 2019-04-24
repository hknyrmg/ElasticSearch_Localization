using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ElasticSearch_Localization.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ElasticSearch_Localization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddLocalization(options => {
                options.ResourcesPath = "Resources";
            });

            services.AddMvc().AddViewLocalization().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton(x => new ConnectionSettings(new Uri("http://localhost:9200")));
            services.AddTransient<IEmployeeService, EmployeeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
          
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouter(routes =>
            {
                routes.MapMiddlewareRoute("{culture=tr-TR}/{*mvcRoute}", _app =>
                {
                    var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("tr-TR"),
                new CultureInfo("en-US")
            };

                    var requestLocalizationOptions = new RequestLocalizationOptions
                    {
                        DefaultRequestCulture = new RequestCulture("tr-TR"),
                        SupportedCultures = supportedCultures,
                        SupportedUICultures = supportedCultures
                    };

                    requestLocalizationOptions.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
                    _app.UseRequestLocalization(requestLocalizationOptions);

                    _app.UseMvc(mvcRoutes =>
                    {
                        mvcRoutes.MapRoute(
                            name: "default",
                            template: "{culture=tr-TR}/{controller=Home}/{action=Index}/{id?}");
                    });
                });
            });
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
