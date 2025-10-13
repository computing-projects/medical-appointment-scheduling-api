using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IClientHealthPlansRepository
    {
        Task<IEnumerable<ClientHealthPlans>> GetAllAsync();
        Task<ClientHealthPlans?> GetByIdAsync(int id);
        Task<IEnumerable<ClientHealthPlans>> GetByClientIdAsync(int clientId);
        Task<ClientHealthPlans> CreateAsync(ClientHealthPlans clientHealthPlan);
        Task<ClientHealthPlans?> UpdateAsync(ClientHealthPlans clientHealthPlan);
        Task<bool> DeleteAsync(int id);
    }
}
