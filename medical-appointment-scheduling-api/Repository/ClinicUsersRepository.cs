using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class ClinicUsersRepository : IClinicUsersRepository
    {
        private readonly AppDbContext _db;

        public ClinicUsersRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<ClinicUsers>> GetAllAsync()
        {
            return await _db.ClinicUsers.ToListAsync();
        }

        public async Task<ClinicUsers> GetByIdAsync(int id)
        {
            return await _db.ClinicUsers.FindAsync(id);
        }

        public async Task<IEnumerable<ClinicUsers>> GetByUserIdAsync(int userId)
        {
            return await _db.ClinicUsers
                .Where(cu => cu.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ClinicUsers>> GetByClinicIdAsync(int clinicId)
        {
            return await _db.ClinicUsers
                .Where(cu => cu.ClinicId == clinicId)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(ClinicUsers clinicUser)
        {
            await _db.ClinicUsers.AddAsync(clinicUser);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(ClinicUsers clinicUser)
        {
            _db.ClinicUsers.Update(clinicUser);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var clinicUser = await _db.ClinicUsers.FindAsync(id);
            if (clinicUser != null)
            {
                _db.ClinicUsers.Remove(clinicUser);
                await _db.SaveChangesAsync();
            }
            return true;
        }
    }
}
