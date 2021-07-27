using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechStart.Core;
using TechStart.DbContexts;
using TechStart.Models;

namespace TechStart.Persistence
{
    public class ItemRepo : IItemRepo
    {
        #region Initializer
        private readonly TechDbContext context;
        public ItemRepo(TechDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region public async Task<Item> GetItem(long ItemNumber)
        public async Task<Item> GetItem(long ItemNumber)
        {
            return await context.Items
                .Where(i => i.Status != "D")
                .Include(i => i.ItemVendor)
                .SingleOrDefaultAsync(i => i.ItemNumber == ItemNumber);
        }
        #endregion
        
        #region public async Task<List<Item>> GetItems()
        public async Task<List<Item>> GetItems()
        {
            return await context.Items
                .Where(i => i.Status != "D")
                .Include(i => i.ItemVendor)
                .ToListAsync();
        }
        #endregion

        #region public async void CreateItem(Item Item)
        public async void CreateItem(Item Item)
        {
            await context.AddAsync(Item);
        }
        #endregion
    }
}