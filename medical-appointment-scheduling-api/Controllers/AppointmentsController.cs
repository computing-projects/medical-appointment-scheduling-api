using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using System;
using medical_appointment_scheduling_api.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsRepository _repo;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(IAppointmentsRepository repo, ILogger<AppointmentsController> logger)
        {
            _repo = repo;
            _logger = logger;
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

            var consultas = new List<object>
            {
                new
                {
                    id = 1,
                    status = "Confirmada",
                    doctorPhoto = "",
                    especialidade = "Cardiologia",
                    profissional = "Dr. João Silva",
                    tipo = "Presencial",
                    dataHora = "2023-10-26 - 10:00",
                    doctorName = "João",
                    doctorLastName = "Silva",
                    doctorContact = "(11) 98765-4321",
                    doctorAge = 45,
                    doctorEspecialty = "Cardiologia",
                    doctorCRM = "SP123456",
                    doctorReview = 4,
                    appointmentType = "Presencial",
                    appointmentStatus = "Realizada",
                    reason = "Check-up de rotina e acompanhamento de pressão alta.",
                    history = new[]
                    {
                        new { date = "2022-04-10", type = "Check-up" },
                        new { date = "2021-11-20", type = "Primeira Consulta" }
                    }
                },
                new
                {
                    id = 2,
                    status = "Realizada",
                    profissional = "Dra. Maria Oliveira",
                    especialidade = "Dermatologia",
                    dataHora = "2023-09-15 - 14:30",
                    tipo = "Online",
                    doctorPhoto = "",
                    doctorName = "Maria",
                    doctorLastName = "Oliveira",
                    doctorContact = "(21) 99876-5432",
                    doctorAge = 38,
                    doctorEspecialty = "Dermatologia",
                    doctorCRM = "RJ654321",
                    doctorReview = 5,
                    appointmentType = "Online",
                    appointmentStatus = "Realizada",
                    appointmentLink = "https://meet.google.com/abc-xyz-123",
                    reason = "Avaliação de manchas na pele e orientação para cuidados.",
                    history = new[]
                    {
                        new { date = "2023-03-01", type = "Consulta de Retorno" }
                    }
                }
            };
            var result = await _repo.GetByIdAsync(id);
            return Ok(consultas);
        }

        [HttpPost("Schedule")]
        public async Task<IActionResult> ScheduleAsync([FromBody] Appointments appointment)
        {
            var result = await _repo.AgendarAsync(appointment);
            return Ok(result);
        }

        [HttpPut("Reschedule/{id}")]
        public async Task<IActionResult> RescheduleAsync(int id, [FromBody] Appointments appointment)
        {
            var result = await _repo.RemarcarAsync(appointment);
            return Ok(result);
        }

        [HttpDelete("Cancel/{id}")]
        public async Task<IActionResult> CancelAsync(int id)
        {
            var result = await _repo.CancelAsync(id);
            return Ok(result);
        }
    }
}
