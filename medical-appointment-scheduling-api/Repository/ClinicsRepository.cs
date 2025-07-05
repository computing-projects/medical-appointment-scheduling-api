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
            await _db.Clinics.AddAsync(clinic);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Clinics clinic)
        {
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
                          join cU in _db.ClinicUsers on doctor.user_id equals cU.user_id
                          join clinic in _db.Clinics on cU.clinic_id equals clinic.id
                          where clinic.id == Id
                          select doctor).DistinctBy(d => d.id).ToListAsync();
        }
    }
}
