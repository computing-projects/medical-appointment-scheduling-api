using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Models.DTO;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IClientsRepository
    {
        Task<IEnumerable<Clients>> GetAllAsync();
        Task<Clients> GetByIdAsync(int id);
        Task<Clients?> GetByUserIdAsync(int userId);
        Task<bool> CreateAsync(Clients client);
        Task<bool> UpdateAsync(Clients client);
        Task<bool> DeleteAsync(int id);
        Task<List<Appointments>> GetPastAppointments(int id);
        Task<List<Appointments>> GetPendingAppointments(int id);
        Task<ClientProfileDto?> GetProfileAsync(int clientId);
        Task<bool> UpdateProfileAsync(int clientId, ClientProfileDto profileDto);
    }
}
