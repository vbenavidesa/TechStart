using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechStart.Core;
using TechStart.DbContexts;
using TechStart.Models;

namespace TechStart.Persistence
{
    public class PharmacyRepo : IPharmacyRepo
    {
        #region Initializer
        private readonly TechDbContext context;
        public PharmacyRepo(TechDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region public async Task<Pharmacy> GetPharmacy(long Id)
        public async Task<Pharmacy> GetPharmacy(long Id)
        {
            return await context.Pharmacies
                .Where(p => p.Status != "D")
                .Include(p => p.Hospital)
                .SingleOrDefaultAsync(c => c.Id == Id);
        }
        #endregion
        
        #region public async Task<List<Pharmacy>> GetPharmacies()
        public async Task<List<Pharmacy>> GetPharmacies()
        {
            return await context.Pharmacies
                .Where(p => p.Status != "D")
                .Include(p => p.Hospital)
                .ToListAsync();
        }
        #endregion

        #region public async void CreatePharmacy(Pharmacy Pharmacy)
        public async void CreatePharmacy(Pharmacy Pharmacy)
        {
            await context.AddAsync(Pharmacy);
        }
        #endregion
    }
}