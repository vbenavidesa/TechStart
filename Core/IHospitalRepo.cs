using System.Collections.Generic;
using System.Threading.Tasks;
using TechStart.Models;

namespace TechStart.Core
{
    public interface IHospitalRepo
    {
        Task<Hospital> GetHospital(long Id);
        Task<List<Hospital>> GetHospitals();
        void CreateHospital(Hospital Hospital);
    }
}