using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using System.Runtime.CompilerServices;
using medical_appointment_scheduling_api.Repositories;
using Microsoft.Identity.Client;
using medical_appointment_scheduling_api.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorsRepository _repo;
        private readonly ILogger<DoctorsController> _logger;

        public DoctorsController(IDoctorsRepository repo, ILogger<DoctorsController> logger)
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
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var result = await _repo.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetByUserId/{userId}")]
        public async Task<IActionResult> GetByUserIdAsync([FromRoute] int userId)
        {
            var result = await _repo.GetByUserIdAsync(userId);
            if (result == null)
                return NotFound(new { error = "Doctor not found", message = $"No doctor found for user ID {userId}" });
            
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] Doctors doctor)
        {
            var result = await _repo.CreateAsync(doctor);
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Doctors doctor)
        {
            var result = await _repo.UpdateAsync(doctor);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _repo.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("GetDoctorsByFilter")]
        public async Task<IActionResult> GetDoctorsByFilter([FromBody] FiltroMedicos Filtro)
        {
            var result = await _repo.GetDoctorsByFilter(Filtro);
            return Ok(result);
        }
    }
}
