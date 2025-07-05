using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using medical_appointment_scheduling_api.Repositories;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsRepository _repo;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(IClientsRepository repo, ILogger<ClientsController> logger)
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
        public async Task<IActionResult> CreateAsync([FromBody] Clients client)
        {
            var result = await _repo.CreateAsync(client);
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Clients client)
        {
            var result = await _repo.UpdateAsync(client);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _repo.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("GetConsultasAntigas/{id}")]
        public async Task<IActionResult> GetConsultarAntigas([FromQuery]int id)
        {
            var result = await _repo.GetConsultasAntigas(id);
            return Ok(result);
        }

        [HttpGet("GetConsultasPendentes/{id}")]
        public async Task<IActionResult> GetConsultarPendentes([FromQuery] int id)
        {
            var result = await _repo.GetConsultasPendentes(id);
            return Ok(result);
        }
    }
}
