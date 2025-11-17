using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IWaitlistRepository
    {
        Task<IEnumerable<Waitlist>> GetAllAsync();
        Task<IEnumerable<Waitlist>> GetByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Waitlist>> GetByClientIdAsync(int clientId);
        Task<bool> JoinWaitlistAsync(Waitlist waitlist);
        Task<bool> LeaveWaitlistAsync(int Id);
    }
}
