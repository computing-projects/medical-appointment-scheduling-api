using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class WaitlistRepository : IWaitlistRepository
    {
        private readonly AppDbContext _db;

        public WaitlistRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Waitlist>> GetAllAsync()
        {
            return await _db.Waitlist.ToListAsync();
        }

        public async Task<IEnumerable<Waitlist>> GetByDoctorIdAsync(int doctorId)
        {
            return await _db.Waitlist
                .Where(w => w.DoctorId == doctorId)
                .OrderBy(w => w.Position)
                .ToListAsync();
        }

        public async Task<IEnumerable<Waitlist>> GetByClientIdAsync(int clientId)
        {
            return await _db.Waitlist
                .Where(w => w.ClientId == clientId)
                .OrderBy(w => w.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> JoinWaitlistAsync(Waitlist waitlist)
        {
            // Check if client is already on waitlist for this doctor
            var existing = await _db.Waitlist
                .FirstOrDefaultAsync(w => w.ClientId == waitlist.ClientId 
                    && w.DoctorId == waitlist.DoctorId 
                    && w.Status == SystemEnums.WaitlistStatus.Pending);
            
            if (existing != null)
            {
                return false; // Already on waitlist
            }

            // Calculate position: next position for this doctor (and clinic if specified)
            var query = _db.Waitlist
                .Where(w => w.DoctorId == waitlist.DoctorId 
                    && w.Status == SystemEnums.WaitlistStatus.Pending);
            
            if (waitlist.ClinicId.HasValue)
            {
                query = query.Where(w => w.ClinicId == waitlist.ClinicId || w.ClinicId == null);
            }
            
            // Get max position or 0 if no records exist
            var maxPosition = await query
                .Select(w => (int?)w.Position)
                .MaxAsync() ?? 0;
            
            waitlist.Position = maxPosition + 1;
            waitlist.Status = SystemEnums.WaitlistStatus.Pending;
            waitlist.CreatedAt = DateTimeOffset.UtcNow;
            waitlist.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _db.Waitlist.AddAsync(waitlist);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LeaveWaitlistAsync(int id)
        {
            var waitlist = await _db.Waitlist.FindAsync(id);
            if (waitlist != null)
            {
                _db.Waitlist.Remove(waitlist);
                await _db.SaveChangesAsync();
            }
            return true;
        }
    }
}
