using System.Collections.Generic;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Models.DTO;

namespace medical_appointment_scheduling_api.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Users>> GetAllAsync();
        Task<Users> GetByIdAsync(int id);
        Task<bool> RegisterAsync(Users user);
        Task<bool> DeleteAsync(int id);
        Task<bool> ResetPasswordAsync(RedefinirSenhaDto senha);
        Task<Users> GetByEmailAsync(string Email);
    }
}
