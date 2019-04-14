using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Internship_Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using System;
using System.Threading.Tasks;

namespace Internship_Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /*Name:ConfigureServices
         * Purpose: This method gets called by the runtime. Use this method to add services to the container. A service
         * is a reusable component that allows our app to have functionality.
         */
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
            
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
            });
            
             services.ConfigureApplicationCookie(options =>
             {
                 options.LoginPath = $"/Identity/Account/Login";
                 options.LogoutPath = $"/Identity/Account/Logout";
                 options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
             });
            services.AddMvc();     
        }

        /*Name:Configure
         * Purpose:This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
         * The services IHostingEnvironement and IServiceProvider are injectected
         */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            //create a default route to sign in controller
            app.UseMvc(routes =>
            {
                    routes.MapRoute(
                    name: "default",
                    template: "{controller=SignIn}/{action=Index}/{id?}");
            });
            CreateUserRoles(services).Wait();//go to the function CreateUserRoles
        }




        /*Name:CreateUserRoles
        * Purpose:This method creates the roles Student, Admin, StudentServices, FacultyOfRec, and Employer.
        * The Admin role is assigned to the user in the Identity user variable. The administrators email will be
        * put in there, currently Celeste Tiller's email.
        */
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //Assign Admin role to the main User here we have given our newly registered 
            //login id for Admin management



            IdentityUser user = await UserManager.FindByEmailAsync("taitem2@winthrop.edu");//the admin's email will be put here

            var User = new IdentityUser();
            await UserManager.AddToRoleAsync(user, "Admin");


            // creating Creating student role     
            roleCheck = await RoleManager.RoleExistsAsync("Student");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Student"));
            }

            // creating Creating studentServices role     
            roleCheck = await RoleManager.RoleExistsAsync("StudentServices");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("StudentServices"));
            }

            // creating Creating FacultyofRecord role     
            roleCheck = await RoleManager.RoleExistsAsync("FacultyOfRec");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("FacultyOfRec"));
            }

            // creating Creating Employer role     
            roleCheck = await RoleManager.RoleExistsAsync("Employer");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Employer"));
            }
        }
    }
}

