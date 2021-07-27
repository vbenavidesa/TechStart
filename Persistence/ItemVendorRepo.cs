using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechStart.Core;
using TechStart.DbContexts;
using TechStart.Models;

namespace TechStart.Persistence
{
    public class ItemVendorRepo : IItemVendorRepo
    {
        #region Initializer
        private readonly TechDbContext context;
        public ItemVendorRepo(TechDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region public async Task<ItemVendor> GetItemVendor(long Id)
        public async Task<ItemVendor> GetItemVendor(long Id)
        {
            return await context.ItemVendors
                .Where(c => c.Status != "D")
                .SingleOrDefaultAsync(c => c.Id == Id);
        }
        #endregion
        
        #region public async Task<List<ItemVendor>> GetItemVendors()
        public async Task<List<ItemVendor>> GetItemVendors()
        {
            return await context.ItemVendors
                .Where(c => c.Status != "D")
                .ToListAsync();
        }
        #endregion

        #region public async void CreateItemVendor(ItemVendor ItemVendor)
        public async void CreateItemVendor(ItemVendor ItemVendor)
        {
            await context.AddAsync(ItemVendor);
        }
        #endregion
    }
}