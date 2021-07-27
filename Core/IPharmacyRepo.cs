using System.Collections.Generic;
using System.Threading.Tasks;
using TechStart.Models;

namespace TechStart.Core
{
    public interface IPharmacyRepo
    {
        Task<Pharmacy> GetPharmacy(long Id);
        Task<List<Pharmacy>> GetPharmacies();
        void CreatePharmacy(Pharmacy Pharmacy);
    }
}