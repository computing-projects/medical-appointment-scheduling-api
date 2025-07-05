using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface ISchedulesRepository
    {
        Task<IEnumerable<Schedules>> GetAllAsync();
        Task<Schedules> GetByIdAsync(int id);
        Task<bool> CreateAsync(Schedules schedule);
        Task<bool> UpdateAsync(Schedules schedule);
        Task<bool> DeleteAsync(int id);
    }
}
