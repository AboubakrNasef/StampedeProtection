using Microsoft.EntityFrameworkCore;
using StampedeProtection.Api.Shared.Domain;
using System.Collections.Generic;

namespace StampedeProtection.Api.Shared.InfraStructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
            optionsBuilder.EnableSensitiveDataLogging(false);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product 1", Price = 10.0m },
                new Product { Id = 2, Name = "Product 2", Price = 20.0m },
                new Product { Id = 3, Name = "Product 3", Price = 30.0m },
                new Product { Id = 4, Name = "Product 4", Price = 40.0m },
                new Product { Id = 5, Name = "Product 5", Price = 50.0m }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
