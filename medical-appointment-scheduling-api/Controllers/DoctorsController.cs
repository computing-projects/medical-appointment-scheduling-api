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

        [HttpGet("GetCalendarById/{id}")]
        public async Task<IActionResult> GetCalendarById([FromQuery] int id) 
        {
            var mockAppointments = new Dictionary<string, object[]>
            {
                ["2025-10-10"] = new object[]
                {
                    new
                    {
                        id = 1,
                        time = "10:00",
                        client = "Alice Souza",
                        type = "Presencial",
                        status = "Confirmada",
                        specialty = "Cardiologia",
                        duration = "30 min"
                    },
                    new
                    {
                        id = 2,
                        time = "14:00",
                        client = "Bob Lima",
                        type = "Online",
                        status = "Pendente",
                        specialty = "Clínico Geral",
                        duration = "45 min"
                    },
                    new
                    {
                        id = 7,
                        time = "16:00",
                        client = "Fernanda Silva",
                        type = "Presencial",
                        status = "Confirmada",
                        specialty = "Cardiologia",
                        duration = "30 min"
                    }
                },
                ["2025-10-15"] = new object[]
                {
                    new
                    {
                        id = 3,
                        time = "09:30",
                        client = "Charlie Brown",
                        type = "Presencial",
                        status = "Confirmada",
                        specialty = "Pediatria",
                        duration = "40 min"
                    },
                    new
                    {
                        id = 8,
                        time = "11:00",
                        client = "Rodrigo Alves",
                        type = "Online",
                        status = "Confirmada",
                        specialty = "Clínico Geral",
                        duration = "30 min"
                    }
                },
                ["2025-10-26"] = new object[]
                {
                    new
                    {
                        id = 4,
                        time = "10:00",
                        client = "Ana Paula Costa",
                        type = "Presencial",
                        status = "Realizada",
                        specialty = "Cardiologia",
                        duration = "30 min"
                    },
                    new
                    {
                        id = 5,
                        time = "11:00",
                        client = "Carlos Eduardo Lima",
                        type = "Online",
                        status = "Confirmada",
                        specialty = "Clínico Geral",
                        duration = "45 min"
                    },
                    new
                    {
                        id = 9,
                        time = "14:30",
                        client = "Juliana Mendes",
                        type = "Presencial",
                        status = "Pendente",
                        specialty = "Pediatria",
                        duration = "30 min"
                    }
                },
                ["2025-11-05"] = new object[]
                {
                    new
                    {
                        id = 6,
                        time = "16:00",
                        client = "Maria Clara",
                        type = "Presencial",
                        status = "Pendente",
                        specialty = "Cardiologia",
                        duration = "30 min"
                    }
                }
            };

            return Ok(mockAppointments);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var doctorProfile = new
            {
                photo = "",
                name = "João",
                lastName = "Silva",
                contact = "(XX) XXXXX-XXXX",
                age = 42,
                specialty = "Cardiologista",
                crm = "CRM/SP 123456",
                acceptedPlans = new[] { "Unimed", "Bradesco Saúde", "SulAmérica" },
                availableHoursToday = "09:00 - 12:00, 14:00 - 18:00",
                email = "dr.joao@example.com",
                city = "São Paulo",
                cep = "XXXXX-XXX",
                address = "Rua Principal, 123 - Centro",
                bio = "Cardiologista com mais de 15 anos de experiência, especializado em arritmias e doenças cardíacas congênitas. Atende com foco na prevenção e no bem-estar do paciente.",
                review = 4.8,
                currentMonthStats = new
                {
                    consultas = 50,
                    exames = 25,
                    cirurgias = 3,
                    procedimentos = 10,
                    agendamentosCanceladosOuFaltados = 5
                }
            };
            var result = await _repo.GetByIdAsync(id);
            return Ok(doctorProfile);
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
