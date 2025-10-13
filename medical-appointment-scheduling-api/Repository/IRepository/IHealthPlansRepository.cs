using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IHealthPlansRepository
    {
        Task<IEnumerable<HealthPlans>> GetAllAsync();
        Task<HealthPlans?> GetByIdAsync(int id);
        Task<HealthPlans> CreateAsync(HealthPlans healthPlan);
        Task<HealthPlans?> UpdateAsync(HealthPlans healthPlan);
        Task<bool> DeleteAsync(int id);
    }
}
