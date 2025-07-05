using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IAppointmentsRepository
    {
        Task<IEnumerable<Appointments>> GetAllAsync();
        Task<Appointments> GetByIdAsync(int id);
        Task<bool> AgendarAsync(Appointments appointment);
        Task<bool> RemarcarAsync(Appointments appointment);
        Task<bool> CancelAsync(int id);
    }
}
