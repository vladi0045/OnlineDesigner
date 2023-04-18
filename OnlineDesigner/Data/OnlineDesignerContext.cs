using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineDesigner.Models;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace OnlineDesigner.Data
{
    public class OnlineDesignerContext : IdentityDbContext<User>
    {
        public OnlineDesignerContext(DbContextOptions<OnlineDesignerContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole("Administrator") { NormalizedName = "ADMINISTRATOR" },
                new IdentityRole("Customer") { NormalizedName = "CUSTOMER" }
                );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> Item { get; set; } = default!;
        public DbSet<Models.Type> Type { get; set; } = default!;
        public DbSet<Design> Design { get; set; }
        public DbSet<Order> Order { get; set; }
    }
}
