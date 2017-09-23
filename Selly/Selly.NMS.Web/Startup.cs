using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Selly.NMS.Web.Models;
using AutoMapper;

namespace Selly.NMS.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Instance = Configuration;
        }

        public IConfigurationRoot Configuration { get; }
        public static IConfigurationRoot Instance { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<IDeviceRepository, MockDeviceRepository>();

            // Add framework services.
            services.AddDbContext<AppIdentityDbContext>(options => 
                options.UseSqlServer(Configuration["Data:ConnectionStrings:ConnectionString"])
            );

            services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(Configuration["Data:ConnectionStrings:ConnectionString"])
            );

            services.AddTransient<IDeviceRepository, DbDeviceRepository>();
            services.AddTransient<IEventsRepository, DbEventsRepository>();
            services.AddTransient<IPolicyRepository, DbPolicyRepository>();

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            Mapper.Initialize((mappings) =>
            {
                mappings.CreateMap<Selly.NMS.API.DTO.PacketDroppedEvent, Models.PacketDroppedEvent>();
                mappings.CreateMap<Selly.Agent.Generic.Models.Rule, Models.PolicyRule>().ReverseMap();
                mappings.CreateMap<Models.PolicyRule, ViewModels.PolicyRule.PolicyRuleVM>().ReverseMap();
            });

            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();

            services.AddSignalR(options =>
            {
                options.Hubs.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseWebSockets();
            app.UseSignalR();
            app.UseIdentity();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "rules",
                    template: "Devices/{id}/{controller:regex(Rules)}/{name?}/{action=Index}");

                routes.MapRoute(
                    name: "policyRules",
                    template: "Policy/{id}/{controller:regex(PolicyRules)}/{ruleId?}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
