using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]
[Authorize]
public class HealthPlansController : ControllerBase
{
    private readonly IHealthPlansRepository _healthPlansRepository;

    public HealthPlansController(IHealthPlansRepository healthPlansRepository)
    {
        _healthPlansRepository = healthPlansRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var healthPlans = await _healthPlansRepository.GetAllAsync();
        return Ok(healthPlans);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var healthPlan = await _healthPlansRepository.GetByIdAsync(id);
        if (healthPlan == null)
            return NotFound();
        return Ok(healthPlan);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HealthPlans healthPlan)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdHealthPlan = await _healthPlansRepository.CreateAsync(healthPlan);
        return CreatedAtAction(nameof(GetById), new { id = createdHealthPlan.Id }, createdHealthPlan);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] HealthPlans healthPlan)
    {
        if (id != healthPlan.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedHealthPlan = await _healthPlansRepository.UpdateAsync(healthPlan);
        if (updatedHealthPlan == null)
            return NotFound();

        return Ok(updatedHealthPlan);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _healthPlansRepository.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
