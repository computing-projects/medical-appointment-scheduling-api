using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class AnamneseRepository : IAnamneseRepository
    {
        private readonly AppDbContext _db;

        public AnamneseRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Anamnese>> GetAllAsync()
        {
            return await _db.Anamnese.ToListAsync();
        }

        public async Task<Anamnese?> GetByIdAsync(int id)
        {
            return await _db.Anamnese.FindAsync(id);
        }

        public async Task<Anamnese?> GetByClientIdAsync(int clientId)
        {
            return await _db.Anamnese
                .FirstOrDefaultAsync(a => a.ClientId == clientId);
        }

        public async Task<Anamnese> CreateAsync(Anamnese anamnese)
        {
            // Check if client already has an anamnese
            var existingAnamnese = await _db.Anamnese
                .FirstOrDefaultAsync(a => a.ClientId == anamnese.ClientId);
            
            if (existingAnamnese != null)
            {
                throw new InvalidOperationException($"Client {anamnese.ClientId} already has an anamnese record. Use update instead.");
            }

            anamnese.CreatedAt = DateTimeOffset.UtcNow;
            anamnese.UpdatedAt = DateTimeOffset.UtcNow;
            _db.Anamnese.Add(anamnese);
            await _db.SaveChangesAsync();
            return anamnese;
        }

        public async Task<Anamnese?> UpdateAsync(Anamnese anamnese)
        {
            var existingAnamnese = await _db.Anamnese.FindAsync(anamnese.Id);
            if (existingAnamnese == null)
                return null;

            existingAnamnese.MedicalHistory = anamnese.MedicalHistory;
            existingAnamnese.Allergies = anamnese.Allergies;
            existingAnamnese.Notes = anamnese.Notes;
            existingAnamnese.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync();
            return existingAnamnese;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var anamnese = await _db.Anamnese.FindAsync(id);
            if (anamnese == null)
                return false;

            _db.Anamnese.Remove(anamnese);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
