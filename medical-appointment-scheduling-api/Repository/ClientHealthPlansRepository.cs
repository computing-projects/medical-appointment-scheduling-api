using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class ClientHealthPlansRepository : IClientHealthPlansRepository
    {
        private readonly AppDbContext _db;

        public ClientHealthPlansRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<ClientHealthPlans>> GetAllAsync()
        {
            return await _db.ClientHealthPlans.ToListAsync();
        }

        public async Task<ClientHealthPlans?> GetByIdAsync(int id)
        {
            return await _db.ClientHealthPlans.FindAsync(id);
        }

        public async Task<IEnumerable<ClientHealthPlans>> GetByClientIdAsync(int clientId)
        {
            return await _db.ClientHealthPlans
                .Where(chp => chp.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<ClientHealthPlans> CreateAsync(ClientHealthPlans clientHealthPlan)
        {
            _db.ClientHealthPlans.Add(clientHealthPlan);
            await _db.SaveChangesAsync();
            return clientHealthPlan;
        }

        public async Task<ClientHealthPlans?> UpdateAsync(ClientHealthPlans clientHealthPlan)
        {
            var existingClientHealthPlan = await _db.ClientHealthPlans.FindAsync(clientHealthPlan.Id);
            if (existingClientHealthPlan == null)
                return null;

            existingClientHealthPlan.ClientId = clientHealthPlan.ClientId;
            existingClientHealthPlan.HealthPlanId = clientHealthPlan.HealthPlanId;

            await _db.SaveChangesAsync();
            return existingClientHealthPlan;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var clientHealthPlan = await _db.ClientHealthPlans.FindAsync(id);
            if (clientHealthPlan == null)
                return false;

            _db.ClientHealthPlans.Remove(clientHealthPlan);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
