using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace OnTheFly.SalesAPI.Data
{
    public class OnTheFlySalesAPIContext : DbContext
    {
        public OnTheFlySalesAPIContext(DbContextOptions<OnTheFlySalesAPIContext> options)
           : base(options)
        {
        }
        public DbSet<Models.Sale> Sale { get; set; } = default!;
        public DbSet<CanceledSale> CanceledSale { get; set; }
    }

}

