using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IClinicsRepository
    {
        Task<IEnumerable<Clinics>> GetAllAsync();
        Task<Clinics> GetByIdAsync(int id);
        Task<bool> CreateAsync(Clinics clinic);
        Task<bool> UpdateAsync(Clinics clinic);
        Task<bool> DeleteAsync(int id);
        Task<List<Doctors>> GetAllDoctors(int Id);
    }
}
