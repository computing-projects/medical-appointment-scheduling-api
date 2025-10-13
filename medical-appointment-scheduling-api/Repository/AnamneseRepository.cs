using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class AnamneseRepository : IAnamneseRepository
    {
        private readonly AppDbContext _context;

        public AnamneseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Anamnese>> GetAllAsync()
        {
            return await _context.Anamnese.ToListAsync();
        }

        public async Task<Anamnese?> GetByIdAsync(int id)
        {
            return await _context.Anamnese.FindAsync(id);
        }

        public async Task<Anamnese?> GetByClientIdAsync(int clientId)
        {
            return await _context.Anamnese
                .FirstOrDefaultAsync(a => a.ClientId == clientId);
        }

        public async Task<Anamnese> CreateAsync(Anamnese anamnese)
        {
            anamnese.CreatedAt = DateTime.UtcNow;
            anamnese.UpdatedAt = DateTime.UtcNow;
            _context.Anamnese.Add(anamnese);
            await _context.SaveChangesAsync();
            return anamnese;
        }

        public async Task<Anamnese?> UpdateAsync(Anamnese anamnese)
        {
            var existingAnamnese = await _context.Anamnese.FindAsync(anamnese.Id);
            if (existingAnamnese == null)
                return null;

            existingAnamnese.MedicalHistory = anamnese.MedicalHistory;
            existingAnamnese.Allergies = anamnese.Allergies;
            existingAnamnese.Notes = anamnese.Notes;
            existingAnamnese.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingAnamnese;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var anamnese = await _context.Anamnese.FindAsync(id);
            if (anamnese == null)
                return false;

            _context.Anamnese.Remove(anamnese);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
