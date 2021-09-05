using System;
using System.Linq;
using System.Threading.Tasks;
using Mediwatch.Server.Areas.Identity.Data;
using Mediwatch.Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
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
            services.AddDbContext<IdentityDataContext>(options =>
                  options.UseSqlite("Filename=data.db"));
            services.AddControllersWithViews ();

             services.AddIdentity<UserCustom, IdentityRole<Guid>> (options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<IdentityDataContext> ()
                .AddDefaultTokenProviders ();

            services.AddAuthentication ()
                .AddCookie ("CustomClaimsCookie")
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
            
            services.AddMvc().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service) {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory> ().CreateScope ()) {
                serviceScope.ServiceProvider.GetService<IdentityDataContext> ().Database.Migrate ();
                serviceScope.ServiceProvider.GetService<DbContextMediwatch> ().Database.Migrate ();
            }

            var forwardedHeadersOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };
            forwardedHeadersOptions.KnownNetworks.Clear();
            forwardedHeadersOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardedHeadersOptions);

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseWebAssemblyDebugging ();
            } else {
                app.UseExceptionHandler ("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseBlazorFrameworkFiles ();
            app.UseStaticFiles ();

            app.UseRouting ();
            app.UseAuthentication();
            app.UseAuthorization();
            CreateRoles(service).Wait();
            CreateDemoFormations(service).Wait();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
                endpoints.MapFallbackToFile ("index.html");
            });
        }

        private Task CreateDemoFormations(IServiceProvider serviceProvider)
        {
            if (!Configuration.GetSection("DemoFormations").Exists())
                return Task.CompletedTask;

            var dbContext = serviceProvider.GetRequiredService<DbContextMediwatch>();

            foreach (var DemoConf in Configuration.GetSection("DemoFormations").GetChildren()) {
                IQueryable<formation> query = dbContext.formations;

                query = query.Where(e => e.Name.Contains(DemoConf["Name"]));
                var resultSearch = query.ToList();

                if (resultSearch.Any())
                    continue;

                var form = new formation {
                    Name = DemoConf["Name"],
                    Description = DemoConf["Description"],
                    Former = DemoConf["Former"],
                    Target = DemoConf["Target"],
                    OrganizationName = DemoConf["OrganizationName"],
                    Location = DemoConf["Location"],
                    Price = decimal.Parse(DemoConf["Price"]),
                    StartDate = DateTime.Parse(DemoConf["StartDate"]),
                    EndDate = DateTime.Parse(DemoConf["EndDate"]),
                };

                dbContext.formations.Add(form);
            }

            return dbContext.SaveChangesAsync();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding custom roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<UserCustom>>();
            string[] roleNames = { "Admin", "Tutor", "Member" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {

                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
            //creating a super user who could maintain the web app
            var poweruser = new UserCustom
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