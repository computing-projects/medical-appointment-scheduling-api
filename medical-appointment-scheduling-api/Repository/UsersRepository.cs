using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Models.DTO;

namespace medical_appointment_scheduling_api.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _db;

        public UsersRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<Users> GetByIdAsync(int id)
        {
            return await _db.Users.Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> RegisterAsync(Users user)
        {
            try
            {
                user.PasswordHash = EncryptDecrypt.Encrypt(user.PasswordHash);
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Users user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> ResetPasswordAsync(RedefinirSenhaDto redefinir)
        {
            var qtd = await _db.Users.Where(w => w.Id == redefinir.UserId)
                                     .ExecuteUpdateAsync(spc => spc.SetProperty(s => s.PasswordHash, EncryptDecrypt.Encrypt(redefinir.SenhaAtual)));
            return qtd > 0;
        }

        public async Task<Users> GetByEmailAsync(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
                user.PasswordHash = EncryptDecrypt.Decrypt(user.PasswordHash);
            return user;
        }
    }
}
