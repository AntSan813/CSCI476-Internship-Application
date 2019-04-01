using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Internship_Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using System;

namespace Internship_Application
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
            // Add framework services.
             services.AddDbContext<DataContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            
            services.AddDbContext<IdentityContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => {                 
                // Default Lockout settings.                 
                options.Lockout.DefaultLockoutTimeSpan =                    
                TimeSpan.FromMinutes(5);                 
                options.Lockout.MaxFailedAccessAttempts = 5;                 
                options.Lockout.AllowedForNewUsers = true; 

                // Default Password settings.                 
                options.Password.RequireDigit = true;                 
                options.Password.RequireLowercase = true;                 
                options.Password.RequireNonAlphanumeric = true;                 
                options.Password.RequireUppercase = true;                 
                options.Password.RequiredLength = 6;                 
                options.Password.RequiredUniqueChars = 1;             
            })                 
                .AddDefaultUI(UIFramework.Bootstrap4)                 
                .AddRoles<IdentityRole>()                 
                .AddEntityFrameworkStores<IdentityContext>();

          //      services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<IdentityContext>()
              //  .AddDefaultUI(UIFramework.Bootstrap4);
            // .AddDefaultTokenProviders();
            //services.AddMvc();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
               // options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            


            /*
             services.AddIdentity<Internship_Application.Areas.Identity.Data.InternshipApplicationUser, Internship_Application.Areas.Identity.Data.InternshipApplicationRole>()
                //services.AddDefaultIdentity<InternshipApplicationUser>()
                 .AddEntityFrameworkStores<InternshipApplicationIdentityContext>()
                 .AddDefaultTokenProviders();

             services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                 .AddRazorPagesOptions(options =>
                 {
                     options.AllowAreas = true;
                     options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                     options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                 });

             services.ConfigureApplicationCookie(options =>
             {
                 options.LoginPath = $"/Identity/Account/Login";
                 options.LogoutPath = $"/Identity/Account/Logout";
                 options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
             });
             */

            services.AddMvc();
             // using Microsoft.AspNetCore.Identity.UI.Services;
            
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
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=SignIn}/{action=Index}/{id?}");
            //    routes.MapRoute("adminLandingPage", "{controller=landingPage_Admin}/{action=landingPage_Admin}/{id?}");
            });
        }
    }
}
