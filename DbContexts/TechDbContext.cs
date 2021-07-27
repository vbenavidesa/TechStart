using Microsoft.EntityFrameworkCore;
using TechStart.Models;

namespace TechStart.DbContexts
{
    public class TechDbContext : DbContext
    {
        #region DbSets for database
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemVendor> ItemVendors { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<PharmacyInventory> PharmacyInventories { get; set; }
        #endregion

        public TechDbContext(DbContextOptions<TechDbContext> options)
            : base(options)
        {

        }
    }
}