using FurniturePlus.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FurniturePlus.Data
{
    public class FurniturePlusDbContext : IdentityDbContext
    {
        public FurniturePlusDbContext(DbContextOptions<FurniturePlusDbContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Items { get; init; }
        public DbSet<Category> Categories { get; init; }
        public DbSet<Vendor> Vendors { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany(i => i.Items)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
               .Entity<Item>()
               .HasOne(i => i.Vendor)
               .WithMany(i => i.Items)
               .HasForeignKey(i => i.VendorId)
               .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}

