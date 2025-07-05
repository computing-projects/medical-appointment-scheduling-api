using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicsRepository _repo;
        private readonly ILogger<ClinicsController> _logger;

        public ClinicsController(IClinicsRepository repo, ILogger<ClinicsController> logger)
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
        public async Task<IActionResult> CreateAsync([FromBody] Clinics clinic)
        {
            var result = await _repo.CreateAsync(clinic);
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Clinics clinic)
        {
            var result = await _repo.UpdateAsync(clinic);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _repo.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("GetAllMedics/{Id}")]
        public async Task<IActionResult> GetAllMedics([FromQuery] int Id)
        {
            var result = await _repo.GetAllDoctors(Id);
            return Ok(result);
        }   
    }
}
