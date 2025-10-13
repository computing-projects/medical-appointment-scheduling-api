using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class ClientHealthPlansRepository : IClientHealthPlansRepository
    {
        private readonly AppDbContext _context;

        public ClientHealthPlansRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClientHealthPlans>> GetAllAsync()
        {
            return await _context.ClientHealthPlans.ToListAsync();
        }

        public async Task<ClientHealthPlans?> GetByIdAsync(int id)
        {
            return await _context.ClientHealthPlans.FindAsync(id);
        }

        public async Task<IEnumerable<ClientHealthPlans>> GetByClientIdAsync(int clientId)
        {
            return await _context.ClientHealthPlans
                .Where(chp => chp.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<ClientHealthPlans> CreateAsync(ClientHealthPlans clientHealthPlan)
        {
            _context.ClientHealthPlans.Add(clientHealthPlan);
            await _context.SaveChangesAsync();
            return clientHealthPlan;
        }

        public async Task<ClientHealthPlans?> UpdateAsync(ClientHealthPlans clientHealthPlan)
        {
            var existingClientHealthPlan = await _context.ClientHealthPlans.FindAsync(clientHealthPlan.Id);
            if (existingClientHealthPlan == null)
                return null;

            existingClientHealthPlan.ClientId = clientHealthPlan.ClientId;
            existingClientHealthPlan.HealthPlanId = clientHealthPlan.HealthPlanId;

            await _context.SaveChangesAsync();
            return existingClientHealthPlan;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var clientHealthPlan = await _context.ClientHealthPlans.FindAsync(id);
            if (clientHealthPlan == null)
                return false;

            _context.ClientHealthPlans.Remove(clientHealthPlan);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
