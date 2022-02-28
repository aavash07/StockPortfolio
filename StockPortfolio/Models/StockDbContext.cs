using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioManagementSystem.Models
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {

        }

        public DbSet<StockDetails> StockDetails { get; set; }

        public DbSet<Stocks> Stocks { get; set; }


    }
}
