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
                user.PasswordHash = PasswordHasher.Hash(user.PasswordHash);
                user.CreatedAt = DateTimeOffset.UtcNow;
                user.UpdatedAt = DateTimeOffset.UtcNow;
                
                // Set default profile photo if not provided
                if (string.IsNullOrEmpty(user.ProfilePhotoUrl))
                {
                    // Use UI Avatars service to generate a default avatar based on user's name
                    var initials = GetInitials(user.Name);
                    user.ProfilePhotoUrl = $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(user.Name)}&size=200&background=random&color=fff&bold=true";
                }
                
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "U";

            var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
                return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
            
            return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
        }

        public async Task<bool> UpdateAsync(Users user)
        {
            user.UpdatedAt = DateTimeOffset.UtcNow;
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
            var qtd = await _db.Users.Where(w => w.Email == redefinir.Email)
                .ExecuteUpdateAsync(spc => spc.SetProperty(s => s.PasswordHash, PasswordHasher.Hash(redefinir.NovaSenha)));
            return qtd > 0;
        }

        public async Task<Users> GetByEmailAsync(string email)
        {
            // Do not decrypt; password hashes are one-way now
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
