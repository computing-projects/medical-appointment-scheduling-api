using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IDoctorHealthPlansRepository
    {
        Task<IEnumerable<DoctorHealthPlans>> GetAllAsync();
        Task<DoctorHealthPlans> GetByIdAsync(int id);
        Task<bool> CreateAsync(DoctorHealthPlans healthPlan);
        Task<bool> UpdateAsync(DoctorHealthPlans healthPlan);
        Task<bool> DeleteAsync(int id);
        Task<List<DoctorHealthPlans>> GetAllHealthPlansByDoctorIdAsync(int doctorId);
    }
}
