using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class ClinicsRepository : IClinicsRepository
    {
        private readonly AppDbContext _db;

        public ClinicsRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Clinics>> GetAllAsync()
        {
            return await _db.Clinics.ToListAsync();
        }

        public async Task<Clinics> GetByIdAsync(int id)
        {
            return await _db.Clinics.FindAsync(id);
        }

        public async Task<bool> CreateAsync(Clinics clinic)
        {
            clinic.CreatedAt = DateTimeOffset.UtcNow;
            clinic.UpdatedAt = DateTimeOffset.UtcNow;
            await _db.Clinics.AddAsync(clinic);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Clinics clinic)
        {
            clinic.UpdatedAt = DateTimeOffset.UtcNow;
            _db.Clinics.Update(clinic);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var clinic = await _db.Clinics.FindAsync(id);
            if (clinic != null)
            {
                _db.Clinics.Remove(clinic);
                await _db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<Doctors>> GetAllDoctors(int Id)
        {
            return await (from doctor in _db.Doctors
                          join cU in _db.ClinicUsers on doctor.UserId equals cU.UserId
                          join clinic in _db.Clinics on cU.ClinicId equals clinic.Id
                          where clinic.Id == Id
                          select doctor).DistinctBy(d => d.Id).ToListAsync();
        }
    }
}
