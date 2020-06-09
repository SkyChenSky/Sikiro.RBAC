using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sikiro.Nosql.Mongo;
using Sikiro.Repository.Admin;
using Sikiro.Web.Admin.Attribute;
using Sikiro.Web.Admin.Extention;
using Sikiro.Web.Admin.Permission;

namespace Sikiro.Web.Admin
{
    public class Startup
    {
        private const string AuthName = "sikiro.admin.token";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new GolbalExceptionAttribute());
                options.Filters.Add(new GobalModelValidAttribute());
                options.Filters.Add<GlobalAuthorizeAttribute>();
                options.Filters.Add<GobalPermCodeAttribute>();
                options.ModelBinderProviders.Insert(0, new TrimModelBinderProvider());//去除空格
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.Formatting = Formatting.Indented;
                }
            );

            //cookies身份认证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = AuthName;
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.LoginPath = "/Account/Logon";
                    options.LogoutPath = "/Account/Logout";
                    options.SlidingExpiration = true;
                    options.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(Directory.GetCurrentDirectory()));
                });
            services.AddScoped<GlobalAuthorizeAttribute>();
            services.AddScoped<MongoRep>();
            services.AddHttpContextAccessor();

            //基础框架注入
            services.AddSingleton(new MongoRepository(Configuration["MongoDbUrl"]));
            services.AddService();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            OnStarted(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Logon}/{id?}");
            });
        }

        /// <summary>
        /// app初始化
        /// </summary>
        /// <param name="app"></param>
        private static void OnStarted(IApplicationBuilder app)
        {
            PermissionUtil.InitPermission(app);
        }
    }
}
