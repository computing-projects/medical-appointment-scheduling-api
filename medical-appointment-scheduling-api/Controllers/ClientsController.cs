using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using medical_appointment_scheduling_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
            try
            {
                var result = await _repo.CreateAsync(client);
                return Ok(result);
            }
            catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pgEx)
            {
                return pgEx.SqlState switch
                {
                    "23505" => Conflict(new { error = "Duplicate key violation", message = GetDuplicateKeyMessage(pgEx.ConstraintName), constraint = pgEx.ConstraintName }),
                    "23503" => BadRequest(new { error = "Foreign key violation", message = "Referenced user does not exist", constraint = pgEx.ConstraintName }),
                    _ => StatusCode(500, new { error = "Database error", message = "An error occurred while saving the data" })
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating client");
                return StatusCode(500, new { error = "Server error", message = "An unexpected error occurred" });
            }
        }

        private string GetDuplicateKeyMessage(string constraintName)
        {
            return constraintName switch
            {
                "clients_cpf_key" => "A client with this CPF already exists",
                "clients_rg_key" => "A client with this RG already exists",
                "clients_user_id_key" => "This user already has a client profile",
                _ => "Duplicate data found"
            };
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
