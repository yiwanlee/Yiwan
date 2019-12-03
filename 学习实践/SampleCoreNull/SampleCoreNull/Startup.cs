using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SampleCoreNull
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("AppSettings.json");
            Configuration = builder.Build();
        }

        // 运行时将调用此方法。 使用此方法将服务添加到容器。
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // 运行时将调用此方法。使用该方法来配置 HTTP 请求管道。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            app.UseFileServer();
            //app.UseMvcWithDefaultRoute();

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

            app.Run(async (context) =>
            {
                //throw new System.Exception("Throw Exception");

                var req = context.Request;
                await context.Response.WriteAsync("Run Context:" + $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString.ToUriComponent()}");
            });

            //app.UseWelcomePage();
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            //routeBuilder.MapRoute("Default", "{controller}/{action}/{id?}", new { controller = "Home", action = "Index" });
        }
    }
}
