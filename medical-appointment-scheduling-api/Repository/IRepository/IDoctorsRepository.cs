using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Models.DTO;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IDoctorsRepository
    {
        Task<IEnumerable<Doctors>> GetAllAsync();
        Task<Doctors> GetByIdAsync(int id);
        Task<Doctors?> GetByUserIdAsync(int userId);
        Task<bool> CreateAsync(Doctors doctor);
        Task<bool> UpdateAsync(Doctors doctor);
        Task<bool> DeleteAsync(int id);
        Task<List<Doctors>> GetDoctorsByFilter(FiltroMedicos filtro);
    }
}
