using FurniturePlus.Data;
using FurniturePlus.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FurniturePlus.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedCategories(services);
            SeedAdministrator(services);

            return app;
        }
        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<FurniturePlusDbContext>();
            data.Database.Migrate();
        }

        //migrate initial data
        private static void SeedCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<FurniturePlusDbContext>();

            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category {Name = "Kitchen" },
                new Category {Name = "Bedroom" },
                new Category {Name = "Living room" },
            });

            data.SaveChanges();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // roleManager.RoleExistsAsync()  // --> to avoid async operations, we can use task.Run().GetAwaiter()..
            
            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("Administrator"))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = "Administrator" };
                    await roleManager.CreateAsync(role);

                    var user = new IdentityUser
                    {
                        Email = "admin@email.com"
                    };

                    await userManager.CreateAsync(user, "admin123");

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
