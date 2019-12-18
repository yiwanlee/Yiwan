using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleCoreNull.Models;

namespace SampleCoreNull
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        // 运行时将调用此方法。 使用此方法将服务添加到容器。
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddEntityFrameworkSqlite().AddDbContext<CoreDbContext>();

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<CoreDbContext>();

            //配置authorrize
            //services.AddAuthentication(b =>
            //{
            //    b.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    b.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    b.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).
            //AddCookie(b =>
            //{
            //    //登陆地址
            //    b.LoginPath = "/login";
            //    //sid
            //    b.Cookie.Name = "My_SessionId";
            //    // b.Cookie.Domain = "shenniu.core.com";
            //    b.Cookie.Path = "/";
            //    b.Cookie.HttpOnly = true;
            //    b.Cookie.Expiration = new TimeSpan(0, 0, 30);

            //    b.ExpireTimeSpan = new TimeSpan(0, 0, 30);
            //});
        }

        // 运行时将调用此方法。使用该方法来配置 HTTP 请求管道。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer();
            
            app.UseAuthentication();

            app.UseMvc(ConfigureRoutes);

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    //endpoints.MapGet("/", async context =>
            //    //{
            //    //    var req = context.Request;
            //    //    await context.Response.WriteAsync("Endpoints Context:" + $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString.ToUriComponent()}");
            //    //});
            //});

            //app.Run(async (context) =>
            //{
            //    //throw new System.Exception("Throw Exception");

            //    var req = context.Request;
            //    await context.Response.WriteAsync("Run Context:" + $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString.ToUriComponent()}");
            //});

            //app.UseWelcomePage();
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            //routeBuilder.MapRoute("Default", "{controller}/{action}/{id?}", new { controller = "Home", action = "Index" });

            //AppSettings.Init(Configuration);
        }
    }

    //public class AppSettings
    //{
    //    private static IConfiguration Config = null;

    //    public static string Get(string key)
    //    {
    //        if (Config.GetSection(key) != null) return Config.GetSection(key).Value;
    //        return string.Empty;
    //    }

    //    public static void Init(IConfiguration config)
    //    {
    //        Config = config;
    //    }
    //}
}
