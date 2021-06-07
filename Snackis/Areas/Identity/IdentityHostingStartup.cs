using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snackis.Areas.Identity.Data;
using Snackis.Data;

[assembly: HostingStartup(typeof(Snackis.Areas.Identity.IdentityHostingStartup))]
namespace Snackis.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SnackisContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SnackisContextConnection")));

                services.AddDefaultIdentity<SnackisUser>(options => options.SignIn.RequireConfirmedAccount = false)//Ändra till false
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<SnackisContext>();
            });
        }
    }
}