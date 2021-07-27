using System.Collections.Generic;
using System.Threading.Tasks;
using TechStart.Models;

namespace TechStart.Core
{
    public interface IItemVendorRepo
    {
        Task<ItemVendor> GetItemVendor(long Id);
        Task<List<ItemVendor>> GetItemVendors();
        void CreateItemVendor(ItemVendor ItemVendor);
    }
}