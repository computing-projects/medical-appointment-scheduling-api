using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using System;
using medical_appointment_scheduling_api.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsRepository _repo;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(IAppointmentsRepository repo, ILogger<AppointmentsController> logger)
        {
            _repo = repo;
            _logger = logger;
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

        [HttpPost("Schedule")]
        public async Task<IActionResult> ScheduleAsync([FromBody] Appointments appointment)
        {
            var result = await _repo.AgendarAsync(appointment);
            return Ok(result);
        }

        [HttpPut("Reschedule/{id}")]
        public async Task<IActionResult> RescheduleAsync(int id, [FromBody] Appointments appointment)
        {
            var result = await _repo.RemarcarAsync(appointment);
            return Ok(result);
        }

        [HttpDelete("Cancel/{id}")]
        public async Task<IActionResult> CancelAsync(int id)
        {
            var result = await _repo.CancelAsync(id);
            return Ok(result);
        }
    }
}
