using DAL.RapidPay.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.RapidPay.Context
{
    public class RapidPayContext : DbContext
    {
        public DbSet<User> Users{ get; set; }
        public DbSet<CreditCard> CreditCards{ get; set; }

        public RapidPayContext(DbContextOptions<RapidPayContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
