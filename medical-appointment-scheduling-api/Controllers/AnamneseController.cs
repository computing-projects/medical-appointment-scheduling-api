using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;

[ApiController]
[Route("[controller]")]
public class AnamneseController : ControllerBase
{
    private readonly IAnamneseRepository _anamneseRepository;

    public AnamneseController(IAnamneseRepository anamneseRepository)
    {
        _anamneseRepository = anamneseRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var anamneses = await _anamneseRepository.GetAllAsync();
        return Ok(anamneses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var anamnese = await _anamneseRepository.GetByIdAsync(id);
        if (anamnese == null)
            return NotFound();
        return Ok(anamnese);
    }

    [HttpGet("client/{clientId}")]
    public async Task<IActionResult> GetByClientId(int clientId)
    {
        var anamnese = await _anamneseRepository.GetByClientIdAsync(clientId);
        if (anamnese == null)
            return NotFound();
        return Ok(anamnese);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Anamnese anamnese)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdAnamnese = await _anamneseRepository.CreateAsync(anamnese);
        return CreatedAtAction(nameof(GetById), new { id = createdAnamnese.Id }, createdAnamnese);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Anamnese anamnese)
    {
        if (id != anamnese.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedAnamnese = await _anamneseRepository.UpdateAsync(anamnese);
        if (updatedAnamnese == null)
            return NotFound();

        return Ok(updatedAnamnese);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _anamneseRepository.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
