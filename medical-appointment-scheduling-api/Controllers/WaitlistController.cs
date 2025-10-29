using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WaitlistController : ControllerBase
    {
        private readonly IWaitlistRepository _repo;
        private readonly ILogger<WaitlistController> _logger;

        public WaitlistController(IWaitlistRepository repo, ILogger<WaitlistController> logger)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            var result = await _repo.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("JoinWaitlist")]
        public async Task<IActionResult> JoinWaitlist([FromBody] Waitlist waitlist)
        {
            var result = await _repo.JoinWaitlistAsync(waitlist);
            return Ok(result);
        }

        [HttpDelete("LeaveWaitlist/{id}")]
        public async Task<IActionResult> LeaveWaitlist([FromQuery] int id)
        {
            var result = await _repo.LeaveWaitlistAsync(id);
            return Ok(result);
        }
    }
}
