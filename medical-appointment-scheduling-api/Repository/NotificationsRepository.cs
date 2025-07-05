using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly AppDbContext _db;

        public NotificationsRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Notifications>> GetAllAsync()
        {
            return await _db.Notifications.ToListAsync();
        }

        public async Task<Notifications> GetByIdAsync(int id)
        {
            return await _db.Notifications.FindAsync(id);
        }

        public async Task<bool> SendNotification(Notifications notification)
        {
            try
            {
                _db.Notifications.Add(notification);
                await _db.SaveChangesAsync();
                if (notification.Id != 0)
                {
                    await new EmailRepository(_db).SendNotificationAsync(notification);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
                return false;
            }
        }
    }
}
