using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SchedulesController : ControllerBase
    {
        private readonly ISchedulesRepository _repo;
        private readonly ILogger<SchedulesController> _logger;

        public SchedulesController(ISchedulesRepository repo, ILogger<SchedulesController> logger)
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

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] Schedules schedule)
        {
            var result = await _repo.CreateAsync(schedule);
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Schedules schedule)
        {
            var result = await _repo.UpdateAsync(schedule);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _repo.DeleteAsync(id);
            return Ok(result);
        }
    }
}
