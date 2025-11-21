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

            var mockUserProfile = new
            {
                photo = "",
                name = "Nome do Paciente",
                contact = "(XX) XXXXX-XXXX",
                age = 35,
                state = "SP",
                healthPlans = new[] { "Unimed", "Bradesco Saúde" },
                email = "paciente@example.com",
                cpf = "XXX.XXX.XXX-XX",
                city = "São Paulo",
                cep = "XXXXX-XXX",
                consultas = 15,
                exames = 8,
                cirurgias = 1,
                procedimentos = 3,
                agendamentosCanceladosOuFaltados = 2
            };

            var result = await _repo.GetByIdAsync(id);
            return Ok(mockUserProfile);
        }

        [HttpGet("GetByUserId/{userId}")]
        public async Task<IActionResult> GetByUserIdAsync([FromRoute] int userId)
        {
            try
            {
                var result = await _repo.GetByUserIdAsync(userId);
                if (result == null)
                    return NotFound(new { error = "Client not found", message = $"No client found for user ID {userId}" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving client for user ID {UserId}", userId);
                return StatusCode(500, new { error = "Server error", message = "Failed to retrieve client" });
            }
        }

        [HttpPost("Create")]
        [AllowAnonymous]
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

        [HttpGet("GetPastAppointments/{id}")]
        public async Task<IActionResult> GetPastAppointments([FromQuery]int id)
        {
            var result = await _repo.GetPastAppointments(id);
            return Ok(result);
        }

        [HttpGet("GetPendingAppointments/{id}")]
        public async Task<IActionResult> GetPendingAppointments([FromQuery] int id)
        {
            var result = await _repo.GetPendingAppointments(id);
            return Ok(result);
        }

        [HttpGet("{id}/Profile")]
        public async Task<IActionResult> GetProfile(int id)
        {
            try
            {
                var profile = await _repo.GetProfileAsync(id);
                if (profile == null)
                    return NotFound(new { error = "Client not found", message = $"Client with ID {id} does not exist" });

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving client profile for ID {ClientId}", id);
                return StatusCode(500, new { error = "Server error", message = "Failed to retrieve client profile" });
            }
        }

        [HttpPut("{id}/Profile")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] Models.DTO.ClientProfileDto profileDto)
        {
            try
            {
                var success = await _repo.UpdateProfileAsync(id, profileDto);
                if (!success)
                    return NotFound(new { error = "Client not found", message = $"Client with ID {id} does not exist" });

                return Ok(new { message = "Profile updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating client profile for ID {ClientId}", id);
                return StatusCode(500, new { error = "Server error", message = "Failed to update client profile" });
            }
        }
    }
}
