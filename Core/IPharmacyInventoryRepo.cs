using System.Collections.Generic;
using System.Threading.Tasks;
using TechStart.Models;

namespace TechStart.Core
{
    public interface IPharmacyInventoryRepo
    {
        Task<PharmacyInventory> GetPharmacyInventory(long Id);
        Task<List<PharmacyInventory>> GetPharmacyInventories();
        void CreatePharmacyInventory(PharmacyInventory PharmacyInventory);
    }
}