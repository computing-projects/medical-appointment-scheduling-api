using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IAnamneseRepository
    {
        Task<IEnumerable<Anamnese>> GetAllAsync();
        Task<Anamnese?> GetByIdAsync(int id);
        Task<Anamnese?> GetByClientIdAsync(int clientId);
        Task<Anamnese> CreateAsync(Anamnese anamnese);
        Task<Anamnese?> UpdateAsync(Anamnese anamnese);
        Task<bool> DeleteAsync(int id);
    }
}
