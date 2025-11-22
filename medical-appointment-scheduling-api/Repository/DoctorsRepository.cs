using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Models.DTO;

namespace medical_appointment_scheduling_api.Repositories
{
    public class DoctorsRepository : IDoctorsRepository
    {
        private readonly AppDbContext _db;

        public DoctorsRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Doctors>> GetAllAsync()
        {
            return await _db.Doctors.ToListAsync();
        }

        public async Task<Doctors> GetByIdAsync(int id)
        {
            return await _db.Doctors.FindAsync(id);
        }

        public async Task<Doctors?> GetByUserIdAsync(int userId)
        {
            return await _db.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<bool> CreateAsync(Doctors doctor)
        {
            await _db.Doctors.AddAsync(doctor);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Doctors doctor)
        {
            _db.Doctors.Update(doctor);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var doctor = await _db.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _db.Doctors.Remove(doctor);
                await _db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<Doctors>> GetDoctorsByFilter(FiltroMedicos filtro)
        {
            var medicos = await _db.Doctors.Where(w => w.Specialty == filtro.specialty).ToListAsync();
            return medicos;
        }
    }
}
