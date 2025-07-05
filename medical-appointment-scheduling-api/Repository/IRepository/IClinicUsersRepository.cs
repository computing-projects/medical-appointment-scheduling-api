using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IClinicUsersRepository
    {
        Task<IEnumerable<ClinicUsers>> GetAllAsync();
        Task<ClinicUsers> GetByIdAsync(int id);
        Task<bool> CreateAsync(ClinicUsers clinicUser);
        Task<bool> UpdateAsync(ClinicUsers clinicUser);
        Task<bool> DeleteAsync(int id);
    }
}
