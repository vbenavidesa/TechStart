using System.Collections.Generic;
using System.Threading.Tasks;
using TechStart.Models;

namespace TechStart.Core
{
    public interface IItemRepo
    {
        Task<Item> GetItem(long ItemNumber);
        Task<List<Item>> GetItems();
        void CreateItem(Item Item);
    }
}