
using Microsoft.AspNetCore.Hosting;


[assembly: HostingStartup(typeof(FurniturePlus.Areas.Identity.IdentityHostingStartup))]
namespace FurniturePlus.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}