using FurniturePlus.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FurniturePlus.Data
{
    public class FurniturePlusDbContext : IdentityDbContext<IdentityUser>
    {
        public FurniturePlusDbContext(DbContextOptions<FurniturePlusDbContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Items { get; init; }
        public DbSet<Category> Categories { get; init; }
        public DbSet<Vendor> Vendors { get; init; }
        public DbSet<Salesman> Salesmen { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
               .Entity<Item>()
               .HasOne(i => i.Vendor)
               .WithMany(v => v.Items)
               .HasForeignKey(i => i.VendorId)
               .OnDelete(DeleteBehavior.Restrict);
            builder
                .Entity<Salesman>()
                .HasOne(s => s.Vendor)
                .WithMany(v => v.Salesmen)
                .HasForeignKey(s => s.VendorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .Entity<Salesman>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Salesman>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
               

            base.OnModelCreating(builder);
        }
    }
}

