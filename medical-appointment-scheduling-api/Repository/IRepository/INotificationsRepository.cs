using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface INotificationsRepository
    {
        Task<IEnumerable<Notifications>> GetAllAsync();
        Task<Notifications> GetByIdAsync(int id);
        Task<bool> SendNotification(Notifications notification);
    }
}
