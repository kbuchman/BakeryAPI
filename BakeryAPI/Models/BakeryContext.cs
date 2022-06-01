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

        public DbSet<Product> Products { get; set; }
    }
}
