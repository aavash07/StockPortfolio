using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockPortfolio.Areas.Identity.Data;
using StockPortfolio.Data;

[assembly: HostingStartup(typeof(StockPortfolio.Areas.Identity.IdentityHostingStartup))]
namespace StockPortfolio.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<StockPortfolioAuthContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("StockPortfolioAuthContextConnection")));

                services.AddDefaultIdentity<StockPortfolioUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<StockPortfolioAuthContext>();
            });
        }
    }
}