using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechStart.Core;
using TechStart.DbContexts;
using TechStart.Models;

namespace TechStart.Persistence
{
    public class HospitalRepo : IHospitalRepo
    {
        #region Initializer
        private readonly TechDbContext context;
        public HospitalRepo(TechDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region public async Task<Hospital> GetHospital(long Id)
        public async Task<Hospital> GetHospital(long Id)
        {
            return await context.Hospitals
                .Where(c => c.Status != "D")
                .SingleOrDefaultAsync(c => c.Id == Id);
        }
        #endregion
        
        #region public async Task<List<Hospital>> GetHospitals()
        public async Task<List<Hospital>> GetHospitals()
        {
            return await context.Hospitals
                .Where(c => c.Status != "D")
                .ToListAsync();
        }
        #endregion

        #region public async void CreateHospital(Hospital Hospital)
        public async void CreateHospital(Hospital Hospital)
        {
            await context.AddAsync(Hospital);
        }
        #endregion
    }
}