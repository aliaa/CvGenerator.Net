using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using EasyMongoNet;
using Newtonsoft.Json.Serialization;
using System.Linq;
using CvGenerator.Logic;
using System.IO;
using System.Collections.Generic;

namespace CvGenerator
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                });

            services.AddAuthorization(conf =>
            {
                conf.AddPolicy("Admin", policy => policy.RequireClaim("IsAdmin"));
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages()
                .AddNewtonsoftJson(
                    options =>
                    {
                        options.SerializerSettings.Converters.Add(new ObjectIdJsonConverter());
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        // Maintain property names during serialization. See:
                        // https://github.com/aspnet/Announcements/issues/194
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // Add mongodb service:
            var db = new MongoDbContext(Configuration.GetValue<string>("DBName"), Configuration.GetValue<string>("MongoConnString"));
            services.AddSingleton<IDbContext>(db);

            services.AddSingleton(new HtmlToPdfConverter());
            services.AddSingleton(new QrGenerator());

            var env = services.FirstOrDefault(s => typeof(IWebHostEnvironment).IsEquivalentTo(s.ServiceType));
            var templatesPath = Path.Combine((env.ImplementationInstance as IWebHostEnvironment).ContentRootPath, "CvTemplates");
            var templatesCache = new Dictionary<string, Template>();
            foreach (var folder in Directory.GetDirectories(templatesPath))
            {
                var template = new Template(folder);
                templatesCache.Add(template.Name, template);
            }
            services.AddSingleton(templatesCache);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
