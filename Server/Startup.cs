using System;
using System.Linq;
using System.Threading.Tasks;
using Mediwatch.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server;

namespace Mediwatch.Server {
    public class Startup {

        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<DbContextMediwatch> (options =>
                options.UseSqlite ("Filename=data.db"));
            services.AddControllersWithViews ();

            // services.AddIdentity<IdentityUser, IdentityRole> ()
            //     .AddEntityFrameworkStores<IdentityDataContext> ()
            //     .AddDefaultTokenProviders ();

            services.AddAuthentication ()
                .AddCookie ()
                .AddGoogle (googleOptions => {
                    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });

            services.Configure<IdentityOptions> (options => {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie (options => {
                options.Cookie.HttpOnly = false;
                options.Events.OnRedirectToLogin = context => {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service) {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory> ().CreateScope ()) {
                serviceScope.ServiceProvider.GetService<IdentityDataContext> ().Database.Migrate ();
                serviceScope.ServiceProvider.GetService<DbContextMediwatch> ().Database.Migrate ();
            }

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseWebAssemblyDebugging ();
            } else {
                app.UseExceptionHandler ("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }
            CreateRoles(service).Wait();

            app.UseHttpsRedirection ();
            app.UseBlazorFrameworkFiles ();
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
                endpoints.MapFallbackToFile ("index.html");
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding custom roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "Admin", "Tutor", "Member" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            //creating a super user who could maintain the web app
            var poweruser = new IdentityUser
            {
                UserName = Configuration["SuperUser:Username"],
                Email = Configuration["SuperUser:Email"]
               };

            string UserPassword = Configuration["SuperUser:Password"];
            var _user = await UserManager.FindByEmailAsync(Configuration["SuperUser:Email"]);
            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, UserPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }

    }
}