

using FurniturePlus.Data;
using FurniturePlus.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace FurniturePlus.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var data = scopedServices.ServiceProvider.GetService<FurniturePlusDbContext>();

            data.Database.Migrate();
            SeedCategories(data);
            return app;
        }

        //to migrate the initial data - the furniture categories
        private static void SeedCategories(FurniturePlusDbContext database)
        {
            if (database.Categories.Any())
            {
                return;
            }

            database.Categories.AddRange(new[]
            {
                new Category {Name = "Kitchen" },
                new Category {Name = "Bedroom" },
                new Category {Name = "Living room" },
            });

            database.SaveChanges();
        }
    }
}
