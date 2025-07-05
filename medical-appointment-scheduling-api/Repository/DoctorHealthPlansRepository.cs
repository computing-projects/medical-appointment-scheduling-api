using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class DoctorHealthPlansRepository : IDoctorHealthPlansRepository
    {
        private readonly AppDbContext _db;

        public DoctorHealthPlansRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<DoctorHealthPlans>> GetAllAsync()
        {
            return await _db.DoctorHealthPlans.ToListAsync();
        }

        public async Task<DoctorHealthPlans> GetByIdAsync(int id)
        {
            return await _db.DoctorHealthPlans.FindAsync(id);
        }

        public async Task<bool> CreateAsync(DoctorHealthPlans healthPlan)
        {
            await _db.DoctorHealthPlans.AddAsync(healthPlan);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(DoctorHealthPlans healthPlan)
        {
            _db.DoctorHealthPlans.Update(healthPlan);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var healthPlan = await _db.DoctorHealthPlans.FindAsync(id);
            if (healthPlan != null)
            {
                _db.DoctorHealthPlans.Remove(healthPlan);
                await _db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<DoctorHealthPlans>> GetAllHealthPlansByDoctorIdAsync(int doctorId)
        {
            return await _db.DoctorHealthPlans
                .Where(dhp => dhp.doctor_Id == doctorId)
                .ToListAsync();
        }
    }
}
