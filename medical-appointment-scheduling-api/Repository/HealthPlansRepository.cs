using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class HealthPlansRepository : IHealthPlansRepository
    {
        private readonly AppDbContext _context;

        public HealthPlansRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HealthPlans>> GetAllAsync()
        {
            return await _context.HealthPlans.ToListAsync();
        }

        public async Task<HealthPlans?> GetByIdAsync(int id)
        {
            return await _context.HealthPlans.FindAsync(id);
        }

        public async Task<HealthPlans> CreateAsync(HealthPlans healthPlan)
        {
            _context.HealthPlans.Add(healthPlan);
            await _context.SaveChangesAsync();
            return healthPlan;
        }

        public async Task<HealthPlans?> UpdateAsync(HealthPlans healthPlan)
        {
            var existingHealthPlan = await _context.HealthPlans.FindAsync(healthPlan.Id);
            if (existingHealthPlan == null)
                return null;

            existingHealthPlan.Name = healthPlan.Name;

            await _context.SaveChangesAsync();
            return existingHealthPlan;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var healthPlan = await _context.HealthPlans.FindAsync(id);
            if (healthPlan == null)
                return false;

            _context.HealthPlans.Remove(healthPlan);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
