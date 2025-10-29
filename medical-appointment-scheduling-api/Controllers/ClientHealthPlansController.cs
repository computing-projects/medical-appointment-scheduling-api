using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ClientHealthPlansController : ControllerBase
{
    private readonly IClientHealthPlansRepository _clientHealthPlansRepository;
    private readonly ILogger<ClientHealthPlansController> _logger;

    public ClientHealthPlansController(IClientHealthPlansRepository clientHealthPlansRepository, ILogger<ClientHealthPlansController> logger)
    {
        _clientHealthPlansRepository = clientHealthPlansRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clientHealthPlans = await _clientHealthPlansRepository.GetAllAsync();
        return Ok(clientHealthPlans);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var clientHealthPlan = await _clientHealthPlansRepository.GetByIdAsync(id);
        if (clientHealthPlan == null)
            return NotFound();
        return Ok(clientHealthPlan);
    }

    [HttpGet("client/{clientId}")]
    public async Task<IActionResult> GetByClientId(int clientId)
    {
        var clientHealthPlans = await _clientHealthPlansRepository.GetByClientIdAsync(clientId);
        return Ok(clientHealthPlans);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ClientHealthPlans clientHealthPlan)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdClientHealthPlan = await _clientHealthPlansRepository.CreateAsync(clientHealthPlan);
            return CreatedAtAction(nameof(GetById), new { id = createdClientHealthPlan.Id }, createdClientHealthPlan);
        }
        catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pgEx)
        {
            return pgEx.SqlState switch
            {
                "23505" => Conflict(new { error = "Duplicate entry", message = "This client is already associated with this health plan", constraint = pgEx.ConstraintName }),
                "23503" => BadRequest(new { error = "Foreign key violation", message = GetForeignKeyMessage(pgEx.ConstraintName), constraint = pgEx.ConstraintName }),
                _ => StatusCode(500, new { error = "Database error", message = "An error occurred while saving the data" })
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating client health plan");
            return StatusCode(500, new { error = "Server error", message = "An unexpected error occurred" });
        }
    }

    private string GetForeignKeyMessage(string constraintName)
    {
        return constraintName switch
        {
            "client_health_plans_client_id_fkey" => "Client does not exist",
            "client_health_plans_health_plan_id_fkey" => "Health plan does not exist",
            _ => "Referenced data does not exist"
        };
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClientHealthPlans clientHealthPlan)
    {
        if (id != clientHealthPlan.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedClientHealthPlan = await _clientHealthPlansRepository.UpdateAsync(clientHealthPlan);
        if (updatedClientHealthPlan == null)
            return NotFound();

        return Ok(updatedClientHealthPlan);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _clientHealthPlansRepository.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
