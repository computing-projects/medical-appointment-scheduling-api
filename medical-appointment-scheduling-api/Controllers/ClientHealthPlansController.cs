using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;

[ApiController]
[Route("[controller]")]
public class ClientHealthPlansController : ControllerBase
{
    private readonly IClientHealthPlansRepository _clientHealthPlansRepository;

    public ClientHealthPlansController(IClientHealthPlansRepository clientHealthPlansRepository)
    {
        _clientHealthPlansRepository = clientHealthPlansRepository;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClientHealthPlans clientHealthPlan)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdClientHealthPlan = await _clientHealthPlansRepository.CreateAsync(clientHealthPlan);
        return CreatedAtAction(nameof(GetById), new { id = createdClientHealthPlan.Id }, createdClientHealthPlan);
    }

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _clientHealthPlansRepository.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
