using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Models
{
    public class BakeryContext : DbContext 
    {
        public BakeryContext(DbContextOptions<BakeryContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(b => b.Cart)
                .WithOne(i => i.User)
                .HasForeignKey<Cart>(b => b.UserId);

            modelBuilder.Entity<Cart>()
                .HasMany(x => x.Products)
                .WithOne();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
