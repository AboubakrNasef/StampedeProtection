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
            base.OnConfiguring(optionsBuilder);
        }


    }
}
