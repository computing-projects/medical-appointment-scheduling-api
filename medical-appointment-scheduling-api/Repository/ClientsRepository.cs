using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly AppDbContext _db;

        public ClientsRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Clients>> GetAllAsync()
        {
            return await _db.Clients.ToListAsync();
        }

        public async Task<Clients> GetByIdAsync(int id)
        {
            return await _db.Clients.FindAsync(id);
        }

        public async Task<bool> CreateAsync(Clients client)
        {
            await _db.Clients.AddAsync(client);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Clients client)
        {
            _db.Clients.Update(client);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _db.Clients.FindAsync(id);
            if (client != null)
            {
                _db.Clients.Remove(client);
                await _db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<Appointments>> GetConsultasAntigas(int id)
        {
            return await _db.Appointments
                .Where(a => a.ClientId == id && a.AppointmentDatetime < DateTimeOffset.Now)
                .ToListAsync();
        }

        public async Task<List<Appointments>> GetConsultasPendentes(int id)
        {
            return await _db.Appointments
                .Where(a => a.ClientId == id && a.AppointmentDatetime >= DateTimeOffset.Now)
                .ToListAsync();
        }
    }
}
