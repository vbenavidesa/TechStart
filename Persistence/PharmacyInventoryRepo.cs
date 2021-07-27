using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechStart.Core;
using TechStart.DbContexts;
using TechStart.Models;

namespace TechStart.Persistence
{
    public class PharmacyInventoryRepo : IPharmacyInventoryRepo
    {
        #region Initializer
        private readonly TechDbContext context;
        public PharmacyInventoryRepo(TechDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region public async Task<PharmacyInventory> GetPharmacyInventory(long Id)
        public async Task<PharmacyInventory> GetPharmacyInventory(long Id)
        {
            return await context.PharmacyInventories
                .Where(pi => pi.Status != "D")
                .Include(pi => pi.Item)
                .Include(pi => pi.Pharmacy)
                .Include(pi => pi.Item.ItemVendor)
                .SingleOrDefaultAsync(pi => pi.Id == Id);
        }
        #endregion
        
        #region public async Task<List<PharmacyInventory>> GetPharmacyInventories()
        public async Task<List<PharmacyInventory>> GetPharmacyInventories()
        {
            return await context.PharmacyInventories
                .Where(pi => pi.Status != "D")
                .Include(pi => pi.Item)
                .Include(pi => pi.Pharmacy)
                .Include(pi => pi.Item.ItemVendor)
                .ToListAsync();
        }
        #endregion

        #region public async void CreatePharmacyInventory(PharmacyInventory PharmacyInventory)
        public async void CreatePharmacyInventory(PharmacyInventory PharmacyInventory)
        {
            await context.AddAsync(PharmacyInventory);
        }
        #endregion
    }
}