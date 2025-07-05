using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;
using medical_appointment_scheduling_api.Models.DTO;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repo;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUsersRepository repo, ILogger<UsersController> logger)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Users user)
        {
            var result = await _repo.RegisterAsync(user);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _repo.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] RedefinirSenhaDto obj)
        {
            var result = await _repo.ResetPasswordAsync(obj);
            return Ok(result);
        }
    }
}
