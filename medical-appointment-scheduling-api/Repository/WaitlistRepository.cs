using System.Collections.Generic;
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

        public async Task<Waitlist> GetByIdAsync(int id)
        {
            return await _db.Waitlist.FindAsync(id);
        }

        public async Task<bool> JoinWaitlistAsync(Waitlist waitlist)
        {
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
