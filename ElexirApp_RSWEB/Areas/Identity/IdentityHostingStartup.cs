using System;
using ElexirApp_RSWEB.Data;
using ElexirApp_RSWEB.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ElexirApp_RSWEB.Areas.Identity.IdentityHostingStartup))]
namespace ElexirApp_RSWEB.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ElexirApp_RSWEBContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ElexirApp_RSWEBContext")));

             
            });
        }
    }
}