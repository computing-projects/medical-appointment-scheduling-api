using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IReviewsRepository
    {
        Task<IEnumerable<Reviews>> GetAllAsync();
        Task<Reviews> GetByIdAsync(int id);
        Task<bool> EvaluateAsync(Reviews review);
    }
}
