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
        private readonly UserData _userData;
        public DoctorsRepository(UserData userData, AppDbContext context)
        {
            _db = context;
            _userData = userData;
        }

        public async Task<IEnumerable<Doctors>> GetAllAsync()
        {
            return await _db.Doctors.ToListAsync();
        }

        public async Task<Doctors> GetByIdAsync(int id)
        {
            return await _db.Doctors.FindAsync(id);
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
            var qmedicos = _db.Doctors.AsQueryable();

            if (filtro.specialty != 0)
            {
                qmedicos = qmedicos.Where(w => w.Specialty == filtro.specialty);
            }

            if (filtro.FilterByHealthPlans)
            {
                var userHealthPlans = await _db.ClientHealthPlans
                    .Where(c => c.ClientId == _userData.ClientId)
                    .Select(s => s.HealthPlanId).ToListAsync();

                qmedicos =
                    (from d in qmedicos
                     join hp in _db.DoctorHealthPlans on d.Id equals hp.DoctorId
                     where userHealthPlans.Contains(hp.HealthPlanId)
                     select d).AsQueryable();
            }

            return await qmedicos.ToListAsync();
        }
    }
}
