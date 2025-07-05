using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IClientsRepository
    {
        Task<IEnumerable<Clients>> GetAllAsync();
        Task<Clients> GetByIdAsync(int id);
        Task<bool> CreateAsync(Clients client);
        Task<bool> UpdateAsync(Clients client);
        Task<bool> DeleteAsync(int id);
        Task<List<Appointments>> GetConsultasAntigas(int id);
        Task<List<Appointments>> GetConsultasPendentes(int id);
    }
}
