using System;
using Mediwatch.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Mediwatch.Server.Areas.Identity.IdentityHostingStartup))]
namespace Mediwatch.Server.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IdentityDataContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("IdentityDataContextConnection")));

                // services.AddIdentity<IdentityUser, IdentityRole> ()
                // .AddEntityFrameworkStores<IdentityDataContext> ()
                // .AddDefaultTokenProviders ();
            });
        }
    }
}