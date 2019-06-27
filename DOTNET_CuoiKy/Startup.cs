using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using MySql.Data.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DOTNET_CuoiKy.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using DOTNET_CuoiKy.Models.DB;

namespace DOTNET_CuoiKy
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
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            //Luôn Luôn để trước session config
            services.AddAuthentication(o =>{
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>{
                options.LoginPath = "/Login/";
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
            }).AddCookie("Admin", o =>{
                o.ExpireTimeSpan = TimeSpan.FromHours(5);
                o.LoginPath = "/Admin/Authentication/";
            });
            services.AddDbContext<comdatabaseContext>(options => options.UseMySQL(connectionString));
            //Luôn Luôn để sau cookies config
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(5);//You can set Time   
            });
            services.AddDistributedMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Admin",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
