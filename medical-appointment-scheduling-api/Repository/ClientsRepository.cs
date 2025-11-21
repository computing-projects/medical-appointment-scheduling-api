using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Models.DTO;

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

        public async Task<Clients?> GetByUserIdAsync(int userId)
        {
            return await _db.Clients
                .FirstOrDefaultAsync(c => c.UserId == userId);
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

        public async Task<List<Appointments>> GetPastAppointments(int id)
        {
            return await _db.Appointments
                .Where(a => a.ClientId == id && a.AppointmentDatetime < DateTimeOffset.Now)
                .ToListAsync();
        }

        public async Task<List<Appointments>> GetPendingAppointments(int id)
        {
            return await _db.Appointments
                .Where(a => a.ClientId == id && a.AppointmentDatetime >= DateTimeOffset.Now)
                .ToListAsync();
        }

        public async Task<ClientProfileDto?> GetProfileAsync(int clientId)
        {
            var client = await _db.Clients
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
                return null;

            var user = await _db.Users.FindAsync(client.UserId);
            if (user == null)
                return null;

            // Get health plans
            var healthPlans = await _db.ClientHealthPlans
                .Where(chp => chp.ClientId == clientId)
                .Join(_db.HealthPlans, chp => chp.HealthPlanId, hp => hp.Id, (chp, hp) => hp.Name)
                .ToListAsync();

            // Get statistics
            var appointments = await _db.Appointments
                .Where(a => a.ClientId == clientId)
                .ToListAsync();

            var statistics = new ClientStatisticsDto
            {
                Consultations = appointments.Count(a => a.Category == SystemEnums.AppointmentCategory.Consultation && a.Status == SystemEnums.AppointmentStatus.Completed),
                Exams = appointments.Count(a => a.Category == SystemEnums.AppointmentCategory.Exam && a.Status == SystemEnums.AppointmentStatus.Completed),
                Surgeries = appointments.Count(a => a.Category == SystemEnums.AppointmentCategory.Surgery && a.Status == SystemEnums.AppointmentStatus.Completed),
                Procedures = appointments.Count(a => a.Category == SystemEnums.AppointmentCategory.Procedure && a.Status == SystemEnums.AppointmentStatus.Completed),
                CanceledOrMissedAppointments = appointments.Count(a => a.Status == SystemEnums.AppointmentStatus.Canceled || a.Status == SystemEnums.AppointmentStatus.NoShow)
            };

            // Calculate age
            var today = DateTime.Today;
            var birthDate = client.BirthDate.DateTime;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;

            return new ClientProfileDto
            {
                Id = client.Id,
                Photo = user.ProfilePhotoUrl,
                Name = user.Name,
                Contact = client.Phone ?? user.Phone,
                Age = age,
                State = user.State,
                Email = user.Email,
                Cpf = client.Cpf,
                City = user.City,
                Cep = user.Cep,
                HealthPlans = healthPlans,
                Statistics = statistics
            };
        }

        public async Task<bool> UpdateProfileAsync(int clientId, ClientProfileDto profileDto)
        {
            var client = await _db.Clients.FindAsync(clientId);
            if (client == null)
                return false;

            var user = await _db.Users.FindAsync(client.UserId);
            if (user == null)
                return false;

            // Update user info
            user.Name = profileDto.Name;
            user.Email = profileDto.Email;
            user.Phone = profileDto.Contact;
            user.Cep = profileDto.Cep;
            user.City = profileDto.City;
            user.State = profileDto.State;
            user.UpdatedAt = DateTimeOffset.UtcNow;

            // Update client info
            client.Cpf = profileDto.Cpf;
            client.Phone = profileDto.Contact;

            _db.Users.Update(user);
            _db.Clients.Update(client);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
