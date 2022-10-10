using System;
using FarmerAdmin;
using FarmerAdmin.Areas.Identity.Data;
using FarmerAdmin.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace FarmerAdmin
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<FarmerAdminContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("FarmerAdminContextConnection")));

                /*services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<FarmerAdminContext>();*/

                services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                 .AddDefaultUI()
                 .AddEntityFrameworkStores<FarmerAdminContext>()
                 .AddDefaultTokenProviders();
            });
        }
    }
}