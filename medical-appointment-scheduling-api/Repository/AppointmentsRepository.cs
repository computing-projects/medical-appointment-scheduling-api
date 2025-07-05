using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class AppointmentsRepository : IAppointmentsRepository
    {
        private readonly AppDbContext _db;

        public AppointmentsRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Appointments>> GetAllAsync()
        {
            return await _db.Appointments.ToListAsync();
        }

        public async Task<Appointments> GetByIdAsync(int id)
        {
            return await _db.Appointments.FindAsync(id);
        }

        public async Task<bool> AgendarAsync(Appointments appointment)
        {
            await _db.Appointments.AddAsync(appointment);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemarcarAsync(Appointments appointment)
        {
            _db.Appointments.Update(appointment);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelAsync(int id)
        {
            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _db.Appointments.Remove(appointment);
                await _db.SaveChangesAsync();
            }
            return true;
        }
    }
}
