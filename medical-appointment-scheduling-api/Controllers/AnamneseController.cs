using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]
[Authorize]
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

        var mockMedicalHistory = new
        {
            basicInfo = new
            {
                bloodType = "A+",
                weight = "75 kg",
                height = "1.75 m",
                imc = "24.49",
                chronicDiseases = new[] { "Hipertensão (controlada)", "Asma (leve)" },
                hospitalizations = new[]
                {
                    new { date = "2010-03-15", reason = "Apêndicectomia" }
                },
                currentTreatments = new[] 
                { 
                    "Losartana 50mg/dia", 
                    "Bombinha de Salbutamol (conforme necessidade)" 
                }
            },
            allergies = new
            {
                medications = new[] { "Penicilina (reação alérgica grave)" },
                food = new[] { "Amendoim" },
                others = new[] { "Pólen" },
                noKnownAllergies = false
            },
            medications = new[]
            {
                new 
                { 
                    name = "Losartana", 
                    dosage = "50mg", 
                    frequency = "1x ao dia", 
                    reason = "Hipertensão" 
                },
                new 
                { 
                    name = "Salbutamol (bombinha)", 
                    dosage = "2 puffs", 
                    frequency = "Conforme necessidade", 
                    reason = "Asma" 
                }
            },
            pastIllnessesSurgeries = new[]
            {
                new 
                { 
                    type = "Cirurgia", 
                    name = "Apêndicectomia", 
                    date = "2010-03-15" 
                },
                new 
                { 
                    type = "Doença", 
                    name = "Varicela (Catapora)", 
                    date = "Infância" 
                }
            },
            familyHistory = new[]
            {
                new 
                { 
                    relationship = "Pai", 
                    condition = "Hipertensão, Diabetes Tipo 2" 
                },
                new 
                { 
                    relationship = "Mãe", 
                    condition = "Doença Coronariana" 
                }
            },
            lifestyle = new
            {
                smoker = "Não",
                alcohol = "Socialmente",
                physicalActivity = "3x semana (Caminhada)",
                diet = "Equilibrada, com restrição de sal"
            },
            vaccinations = new[]
            {
                new { name = "Gripe", date = "2023-04-01" },
                new { name = "Tétano", date = "2020-08-20" },
                new { name = "COVID-19", date = "2021-06-10 (Dose 1), 2021-08-10 (Dose 2), 2022-03-05 (Reforço)" }
            }
        };

        var anamnese = await _anamneseRepository.GetByClientIdAsync(clientId);
        if (anamnese == null)
            return NotFound();
        return Ok(mockMedicalHistory);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Anamnese anamnese)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdAnamnese = await _anamneseRepository.CreateAsync(anamnese);
            return CreatedAtAction(nameof(GetById), new { id = createdAnamnese.Id }, createdAnamnese);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = "Duplicate anamnese", message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Server error", message = "An unexpected error occurred" });
        }
    }

    [HttpPut("Update/{id}")]
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

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _anamneseRepository.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
