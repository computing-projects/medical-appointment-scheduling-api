using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class SchedulesRepository : ISchedulesRepository
    {
        private readonly AppDbContext _db;

        public SchedulesRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Schedules>> GetAllAsync()
        {
            return await _db.Schedules.ToListAsync();
        }

        public async Task<Schedules> GetByIdAsync(int id)
        {
            return await _db.Schedules.FindAsync(id);
        }

        public async Task<IEnumerable<Schedules>> GetByDoctorIdAsync(int doctorId)
        {
            return await _db.Schedules
                .Where(s => s.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(Schedules schedule)
        {
            await _db.Schedules.AddAsync(schedule);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Schedules schedule)
        {
            _db.Schedules.Update(schedule);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var schedule = await _db.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _db.Schedules.Remove(schedule);
                await _db.SaveChangesAsync();
            }
            return true;
        }
    }
}
